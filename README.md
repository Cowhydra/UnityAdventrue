9# UnityAdventrue

출시 : https://play.google.com/store/apps/details?id=com.KangYunSeok.UnityChanAdventure
인데.. 로그인 방식 DB에서  FireBase(google, Email,Guest), Gpgs,로 변경하면서 게임씬으로 넘김 및..
DB연동 코드 작성 안함 -> 그저 출시용 
-2023.08.28-


개요

유니티찬이 큐브맵을 돌아다니는 게임 ( ---> 모바일 게임으로 전환
 
개요 메이플2 형식, 몬스터 사냥 및 강화 ( -> 큐브맵 포기, 강화의 경우 패쓰...)


시스템
- 전투 ( 적AI 통일 , BOSS AI ( 거리 및 체력에 따른 패턴 활성 보스 각각 1개씩)
- 퀘스트 ( QuestManager , Json을 활용한 퀘스트 할당 및 생성자를 이용해 저장 ? ( 보상(아이템, 블루다이아, 레드다이아) , 퀘스트 타입( 수집형, 처치형), 타겟( 아이템, 몬스터) 퀘스트 시작 시 퀘스트가 각 이벤트(몬스터죽음, 아이템 추가 )에 자동으로 구독(연동)되게 설정( 아이템 수집 퀘스트의 경우 기존 인벤에 있는지 한번 더 확인)  
- 대난투 ( 적 이 마구 생성되어 맵을 돌아다님 - 경험치 던전 느낌입니다. 최적화를 위해 비동기 씬로딩 + 미리 모든 몬스터 생성 후 제거 하여 풀링 을 사용 )
- 던전 ( 아이템 습득 및  사용 장비 장착 가능 -> FIreBase DB와 연동하여 데이터 관리 


목표
adressable 사용 -> 실패, 빌드오류 및 정보 부족 ( 만약 구현한다고 하더라도, 첫 설계 부터 라벨을 다르게 적용했어야 하고  InstantiateAsync 와 같ㅇ는
함수 사용으로 최대한 리소스 최적화를 진행 했어야 했는데.. 아직 비동기 개념 부족 -> Concurreny in C# 2/e 책 구매 후 보는 중 ) 

sql 사용 -> 단기간 내에 SQL을 공부해서 구현하는 것은 현실적으로 힘들어서 간단한 ( NoSQL 형식 - FIreBase 단기 습득 후 사용 ) 
