#include "lod.h"
#include <math.h>

LOD::LOD(GameObject *mainLOD = nullptr, GameObject *secondLOD = nullptr, GameObject *thirdLOD = nullptr, int listIndex = 0)
{

    LODS.push_back(mainLOD);
    LODS.push_back(secondLOD);
    LODS.push_back(thirdLOD);

    mainListIndex = listIndex;
}

void LOD::CheckLOD(Vec3 cameraPos, Vec3 currentLODpos)
{
    Vec3 tempPos = currentLODpos - cameraPos;
    float currentDistance = sqrt(pow(tempPos.x, 2) + pow(tempPos.z, 2));

    if(currentDistance < 5){
        currentLOD = 0;
    }
    else if(currentDistance > 5 && currentDistance < 10){
        currentLOD = 1;
    }
    else {
        currentLOD = 2;
    }
}

void LOD::ChangeLODType(std::vector <GameObject*> mainList, Vec3 cameraPos, Vec3 currentLODpos)
{
    int currentLODinUse = currentLOD;

    CheckLOD(cameraPos, currentLODpos);

    if(currentLODinUse != currentLOD){
        GameObject *tempGO = mainList.back();
        mainList.pop_back();
        mainList.push_back(tempGO);
        mainList[mainListIndex] = tempGO;
        mainList.pop_back();
        mainList.push_back(LODS[currentLOD]);
        mainListIndex = mainList.size();
    }
}



int LOD::GetListIndex()
{
    return mainListIndex;
}
