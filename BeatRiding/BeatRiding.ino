#include <LiquidCrystal.h>
#include <Wire.h>
#include <Time.h>
//자이로센서
#define Device_Address 0x68
#define PWR_MGMT_1 0x6B
#define SMPLRT_DIV 0x19
#define CONFIG 0x1A
#define GYRO_CONFIG 0x1B
#define INT_ENABLE 0x38
#define ACCEL_XOUT_H 0x3B
#define ACCEL_YOUT_H 0x3D
#define ACCEL_ZOUT_H 0x3F
#define GYRO_XOUT_H 0x43
#define GYRO_YOUT_H 0x45
#define GYRO_ZOUT_H 0x47

//방향
float AcX,AcY, AcZ, Tmp, GyX, GyY, GyZ;
float dt;
float accel_angle_x, accel_angle_y, accel_angle_z;
float gyro_angle_x, gyro_angle_y, gyro_angle_z;
float filtered_angle_x, filtered_angle_y, filtered_angle_z;
float baseAcX, baseAcY, baseAcZ;
float baseGyX, baseGyY, baseGyZ;
float gyro_x, gyro_y, gyro_z;

unsigned long now = 0;   // 현재 시간 저장용 변수
unsigned long past = 0;

int cnt = 0; //진동모터에 사용

bool check=false; // L버튼
bool check2=false; // R버튼
bool check3=false; // 핸들 왼쪽방향
bool check4=false; // 핸들 오른쪽방향
//속도 
float radius = 32; // 바퀴당 이동 거리를 확인 하기 위해 자전거 바퀴의 반지름을 입력해 줍니다.(Cm 단위)
float circle = (2 * radius * 3.14) / 100;  // 자전거 바퀴의 둘레를 계산(단위를 m로 바꿔주기 위해 100을 나눕니다.)
float bySpeed = 0; // 자전거의 속도
float ckTime = 0;  // 리드 스위치 cheked time 
float uckTime = 0; // 리드 스위치 Unckecked Time
float cycleTime = 0;  // 리드스위치가 인식이 안됬을 시간 부터 인식됬을 때까지의 시간
float distance = 0; // 자전거의 누적 이동 거리
int count = 0;  // 리드스위치의 노이즈를 제거하기 위해 카운트를 넣어줍니다.
boolean temp = 0;  // 리드 스위치가 닫혔는지 확인하는 변수
float tim = 0; //속도 메시지 조절 시간 변수
float tim2 = 0;

void setup(){
  Serial.begin(115200);
  initSensor();
  calibAccelGyro(); // 안정된 상태에서의 가속도 자이로 값 계산
  past = millis();
  pinMode(2, OUTPUT); // 진동
  pinMode(3, OUTPUT); // LEFT 버튼
  pinMode(4, OUTPUT); // RIGHT 버튼
}

void loop(){
    //방향
    readAccelGyro();
    calcDT();
    calcAccelYPR();
    calcGyroYPR();
    calcFilteredYPR();

    if((filtered_angle_z<-10)&&(check3==false)){ // 핸들 왼쪽 W 전송
      Serial.println("W");
      check3=true;
    }
     if((filtered_angle_z>10)&&(check4==false)){ // 핸들 오른쪽 E 전송
      Serial.println("E");
      check4=true;
    }
    if((filtered_angle_z<10)&&(filtered_angle_z>-10)){ // 초기화
       check3=false;
        check4=false;
    }
    
    //버튼
    if((digitalRead(3)==HIGH)&&(check==false)){ // L버튼 누를시 -> "L" 메시지 전송(유니티에서 L버튼 인식시키기 위해 사용)
      Serial.println("L");
      check=true;
      }
    if(digitalRead(3)==LOW){ // L버튼 누를시 -> "L" 메시지 전송(유니티에서 L버튼 인식시키기 위해 사용)
      check=false;
      }
      if((digitalRead(4)==HIGH)&&(check2==false)){ // R버튼 누를시 -> "R" 메시지 전송(유니티에서 R버튼 인식시키기 위해 사용)
      Serial.println("R");
      check2=true;
      }
    if(digitalRead(4)==LOW){ // R버튼 누를시 -> "R" 메시지 전송(유니티에서 R버튼 인식시키기 위해 사용)
      check2=false;
      }
    //진동    
  char data = Serial.read(); // 시리얼 통신 메시지 읽어와 저장
  if(data == 'Z'){
    digitalWrite(2, HIGH); // 시리얼 통신에서 메시지가 "Z"일 때 진동모터 켬
    cnt = 0;
  }
  cnt++;
  if(cnt>100){
    digitalWrite(2, LOW); //진동모터 끔
  }

  //속도
  boolean check = digitalRead(A0); // 리드스위치의 상태를 확인합니다.
  
  if(check == 1 && temp == 0){  // 리드 스위치가 열릴 때(닫힘 -> 열림)
    ckTime = millis();  // 시간을 확인해서 저장합니다.
    temp = 1;  // temp값을 1로 바꿔줍니다.(리드스위치가 열려있는 상태값 저장)
  }
  
  else if(check == 0 && temp == 1 && count > 5){  // 리드 스위치가 닫히고(열림 -> 닫힘), 노이즈 방지 카운트가 5이상일때
    uckTime = millis();  // 시간을 확인해서 저장합니다.
      
    cycleTime = (uckTime - ckTime) / 1000; // 열릴 때 시각과 닫힐 때 시각의 차를 이용하여 바퀴가 한바퀴 돌때 걸린 시간을 계산합니다.
    bySpeed = (circle / cycleTime) * 3.6; // 바퀴가 한바퀴 돌때의 거리와 시간을 가지고 속도를 구해줍니다.(단위는 Km/h입니다.)
    temp = 0; 
    count = 0; 
    distance += circle;  // 한바퀴 돌았으면 이동거리를 누적 이동거리에 더해줍니다.
  } 
  if(check == 1){  // 리드 스위치가 열려있으면 카운트를 1씩 증가 시켜 줍니다.
    count++;
    if(count > 1000){ // 카운트가 5000이 넘어가면(자전거가 멈췄을 때) 속도를 0으로 바꿔줍니다.
      bySpeed = 0;
    }
  }
  tim2 = millis(); //메시지 조절 시간 변수
  if((tim2-tim)>=500){
  Serial.print("V|");   // 속도 값 구분해주기 위해 사용 
  Serial.println(bySpeed);// 시리얼 모니터를 이용하여 속도를 확인합니다.
  tim = millis();
  }
}

void initSensor(){
  Wire.begin();
  Wire.beginTransmission(Device_Address);   // I2C 통신용 어드레스(주소)
  Wire.write(0x6B);    // MPU6050과 통신을 시작하기 위해서는 0x6B번지에    
  Wire.write(0x00); //초기화
  Wire.endTransmission(true);

  //자이로
  Wire.beginTransmission(0x68);        
  Wire.write(0x1B); 
  Wire.write(0x08); //fs_sel = 1 => +- 1000 도/초, 범위가 작으면 섬세하게, 범위가 넓으면 큰 각도변화
  Wire.endTransmission(true);
  
  //가속도
  //Wire.beginTransmission(0x68);        
  //Wire.write(0x1C); 
  //Wire.write(0x08);
  //Wire.endTransmission(true);
  
}

void calibAccelGyro()
{
    float sumAcX = 0, sumAcY = 0, sumAcZ = 0;
    float sumGyX = 0, sumGyY = 0, sumGyZ = 0;
    readAccelGyro();
    for (int i = 0; i < 10; i++)
    {
        readAccelGyro();
        sumAcX += AcX;
        sumAcY += AcY;
        sumAcZ += AcZ;
        sumGyX += GyX;
        sumGyY += GyY;
        sumGyZ += GyZ;
        delay(100);
    }
    baseAcX = sumAcX / 10;
    baseAcY = sumAcY / 10;
    baseAcZ = sumAcZ / 10;
    baseGyX = sumGyX / 10;
    baseGyY = sumGyY / 10;
    baseGyZ = sumGyZ / 10;
    //printf("baseAcX %f\t", baseAcX);
    //printf("baseAcY %f\t", baseAcY);
    //printf("baseAcZ %f\t\n", baseAcZ);
}
void calcDT()
{
    now = millis();
    dt = (now - past) / 1000.0;
    past = now;
}

void readAccelGyro()
{
    Wire.beginTransmission(Device_Address);
    Wire.write(0x3B);   // AcX 레지스터 위치(주소)를 지칭합니다
    Wire.endTransmission(false); //연결 유지
    Wire.requestFrom(Device_Address, 14, true);  // AcX 주소 이후의 14byte의 데이터를 요청
    AcX = Wire.read() << 8 | Wire.read(); //두 개의 나뉘어진 바이트를 하나로 이어 붙여서 각 변수에 저장
    AcY = Wire.read() << 8 | Wire.read();
    AcZ = Wire.read() << 8 | Wire.read();
    Tmp = Wire.read() << 8 | Wire.read();
    GyX = Wire.read() << 8 | Wire.read();
    GyY = Wire.read() << 8 | Wire.read();
    GyZ = Wire.read() << 8 | Wire.read();
}

void calcAccelYPR()
{
    float accel_x, accel_y, accel_z;
    float accel_xz, accel_yz;
    const float RADIANS_TO_DEGREES = 180 / 3.14159;
    accel_x = AcX - baseAcX;
    accel_y = AcY - baseAcY;
    accel_z = AcZ + (16384 - baseAcZ);

    accel_yz = sqrt(pow(accel_y, 2) + pow(accel_z, 2));
    accel_angle_y = atan(-accel_x / accel_yz) * RADIANS_TO_DEGREES;
    accel_xz = sqrt(pow(accel_x, 2) + pow(accel_z, 2));
    accel_angle_x = atan(accel_y / accel_xz) * RADIANS_TO_DEGREES;
    accel_angle_z = 0;
}
void calcGyroYPR()
{
    const float GYROXYZ_TO_DEGREES_PER_SEC = 65;
    gyro_x = (GyX - baseGyX) / GYROXYZ_TO_DEGREES_PER_SEC;
    gyro_y = (GyY - baseGyY) / GYROXYZ_TO_DEGREES_PER_SEC;
    gyro_z = (GyZ - baseGyZ) / GYROXYZ_TO_DEGREES_PER_SEC;
    //자이로 센서의 값을 각속도로 매핑
}
void calcFilteredYPR()
{
    const float ALPHA = 0.96;
    float tmp_angle_x, tmp_angle_y, tmp_angle_z;
    tmp_angle_x = filtered_angle_x + gyro_x * dt; //각속도에서 각도로 변환
    tmp_angle_y = filtered_angle_y + gyro_y * dt;
    tmp_angle_z = filtered_angle_z + gyro_z * dt;
    filtered_angle_x = ALPHA * tmp_angle_x + (1.0 - ALPHA) * accel_angle_x;
    filtered_angle_y = ALPHA * tmp_angle_y + (1.0 - ALPHA) * accel_angle_y;
    filtered_angle_z = tmp_angle_z;
}
