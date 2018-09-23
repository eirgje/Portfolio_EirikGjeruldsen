#ifndef LOD_H
#define LOD_H

#include <gameobject.h>
#include <vec3.h>

//Level-Of-Detail
//This class is to create a less detailed version of a 3D model
//The code have been made so that it should be called in an UpdateFunction in another class.

class LOD
{
public:
    LOD(GameObject *mainLOD, GameObject *secondLOD, GameObject *thirdLOD, int listIndex);

    void ChangeLODType(std::vector <GameObject*> mainList, Vec3 cameraPos, Vec3 currentLODpos);

    int GetListIndex();

private:

    void CheckLOD(Vec3 cameraPos, Vec3 currentLODpos);

    std::vector<GameObject*> LODS;
    int currentLOD = 0;
    int mainListIndex = 0;
};

#endif // LOD_H
