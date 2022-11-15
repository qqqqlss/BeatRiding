# BeatRiding (VR Rhythm Cycle Game)
코로나로 인해 외부 활동의 제한과 신체활동 감소로 체중 증가 등의 문제를 해소할 수 있는 유산소 운동을 만들고자 했습니다. 그러나 사이클, 런닝 등의 유산소 운동은 혼자서 반복해야 하는 지루함을 견뎌야하고, 넓은 공간이 필요하거나 실내에서는 소음이 동반될 수밖에 없다는 단점이 있습니다.

#### 이를 모두 해결할 수 있는 VR을 쓰고 자전거를 타는 리듬게임
## 환경 및 언어
**Language** - C#(Unity), C++(Arduino), PHP(WEB DB)
**Development Environment** - MYSQL, Unity, Arduino, Visual Studio   
**Hardware** - `PC`(최소사양 OS-Windows 7 SP1, CPU-i5-4590, GPU-GTX 970 4GB, RAM-4GB),  
`Arduino UNO 3`(+진동모터, 버튼, 자이로, 리드스위치 센서), `VR`(HTC VIVE), `자전거`(+고정 롤러) 
## 시스템 구성도
![image](https://user-images.githubusercontent.com/54983139/201930131-24d35069-f066-44a9-9a94-673d572cf454.png)
## 담당역할 및 사용한 기술스택
- 5인팀 팀장 
- **Unity 와 Arduino 센서 연동**  
asset store의 ardity를 활용해 시리얼 통신  
시리얼 메시지를 통한 Unity게임 내 동작 구성  
(칼로리 측정, 레일 속도, 방향 전환, 버튼 동작, 충돌 시 진동) 

- **센서값 가공**  
오차가 누적되는 자이로 센서를 상보필터를 통해 보완해 자전거의 방향 측정 (LEFT, STRAIGT, RIGHT)  
자전거의 바퀴에 자석을 달아 프레임에 설치된 리드 스위치가 켜지는 시간 간격을 통해 속도 측정

- **VR 가상자판**
게임 내에 가상자판을 만들어 키보드 없이 닉네임 입력

## 성과
한국전자전 출품, EXPO 우수상 (전공 동아리 EL)  
  
## 시연 영상
[![image](https://user-images.githubusercontent.com/54983139/196341776-873c4f37-fb43-49ac-a468-47704ee69b60.png)](https://youtu.be/taAlZWLYEfU)
[![image](https://user-images.githubusercontent.com/54983139/196342223-48497a72-8f23-4e0b-b8be-13a398d97f3a.png)](https://youtu.be/vOACRT3UKcs)
## 주요 기능
- 장애물 피하고 아이템 먹기
- 칼로리 소모 및 관리
- 이지/하드 난이도 분류
- 콤보와 속도 따른 점수획득
## 서비스 시나리오
![image](https://user-images.githubusercontent.com/54983139/201928956-6d0d884e-f418-47b9-8e37-8ea90276a5c9.png)

