#ifndef SCENEMANAGER_H
#define SCENEMANAGER_H
#include <Qfile>

#include "gameobject.h"


//The scene manger is made to save and load your work.
//This is done through writing and reading a .txt-file.

class SceneManager : protected QOpenGLFunctions
{
public:
    SceneManager();

    void SaveCurrentScene(std::vector<GameObject *> gameobjects, QString filePath);
    void LoadScene(std::vector<GameObject *> &gameobjects, QString filePath, Material *tempMaterial);
};


#endif // SCENEMANAGER_H
