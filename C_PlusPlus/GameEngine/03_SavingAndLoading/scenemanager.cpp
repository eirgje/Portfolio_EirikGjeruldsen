#include "scenemanager.h"
#include <fstream>
#include <ostream>
#include <iostream>
#include <sstream>
#include "transform.h"
#include <qfiledialog>
#include "camera.h"
#include "cube.h"
#include "plane.h"
#include "oktaederball.h"
#include "axesgizmo.h"
#include "circlesphere.h"
#include "skybox.h"
#include "material.h"
#include "objloader.h"


SceneManager::SceneManager()
{

}

void SceneManager::SaveCurrentScene(std::vector<GameObject *> gameobjects, QString filePath)
{
    std::string fileName = filePath.toStdString();
    std::ofstream write (fileName.c_str());

    Transform* transform;

    for(unsigned int i = 0; i < gameobjects.size(); i++)
    {
        qDebug() <<"Writing line: " << QString::number(i);
        if(i < 1){}
        else if (gameobjects[i]->getGameobjectType().toStdString() == "Skybox"){}
        else {
            if(gameobjects[i]->getGameobjectType().toStdString() == "Cube"){
                write << "C " << gameobjects[i]->getGameobjectType().toStdString() << std::endl; // name of object
            }
            else if(gameobjects[i]->getGameobjectType().toStdString() == "Plane"){
                write << "P " << gameobjects[i]->getGameobjectType().toStdString() << std::endl; // name of object
            }
            else if(gameobjects[i]->getGameobjectType().toStdString() == "Ball"){
                write << "B " << gameobjects[i]->getGameobjectType().toStdString() << std::endl; // name of object
            }
            else if(gameobjects[i]->getGameobjectType().toStdString() == "Import"){
                write << "I " << gameobjects[i]->getGameobjectType().toStdString() << std::endl; // name of object
            }
            write << "N " << gameobjects[i]->getName().toStdString() << std::endl;

            transform = gameobjects[i]->mTransform;
            Vec3 position = transform->getPosition();
            Vec3 scale = transform->getScale();
            Vec3 rotation = transform->getRotation();

            write << "T " << position.x << " " << position.y << " " << position.z << " "
                  << scale.x    << " " << scale.y    << " " << scale.z    << " "
                  << rotation.x << " " << rotation.y << " " << rotation.z << std::endl;
            write << "~" << std::endl;
        }

    }
    transform = nullptr;
    write.close();
}

void SceneManager::LoadScene(std::vector<GameObject *> &gameobjects, QString filePath, Material *tempMaterial)
{
    QFile mFile(filePath);
    if(!mFile.open(QFile::ReadOnly | QFile::Text)){
        qDebug() << "could not open file: " << filePath;
        //return;
    }

    QTextStream in(&mFile);
    std::string text;

    GameObject *tempGameObject; // variables for constructing gameobjects
    std::string name;
    float transformNums[9];

    while(!in.atEnd())
    {
        std::stringstream sStream;
        text = in.readLine().toStdString();
        sStream << text;
        std::string lineHeader;
        sStream >> lineHeader;

        qDebug() << QString::fromStdString(lineHeader);

        if(lineHeader == std::string("Count"))
        {
            //not used (yet)
        }

        else if(lineHeader == std::string("MC"))
        {
            tempGameObject = new Camera();
            tempGameObject->mTransform->setPosition(0,0,0);
            tempGameObject->setName("MainCamera");
            if(tempGameObject != nullptr)
                gameobjects.push_back(tempGameObject);

            qDebug() << QString(tempGameObject->getGameobjectType()) << " was added through loading";
        }
        else {
            if(lineHeader == std::string("C"))
            {
                tempGameObject = new Cube(0.0f, 0.0f, 0.0f, " ");

                while(lineHeader != std::string("~"))
                {
                    std::stringstream sStreamLoop;
                    text = in.readLine().toStdString();
                    sStreamLoop << text;
                    sStreamLoop >> lineHeader;

                    if(lineHeader == std::string("N"))
                    {
                        sStreamLoop >> name;
                        tempGameObject->setName(QString::fromStdString(name));
                    }
                    else if(lineHeader == std::string("T"))
                    {
                        std::string transform;
                        for(int x = 0; x < 9; x++) // transform has 9 numbers, always
                        {
                            sStreamLoop >> transform;
                            transformNums[x] = atof(transform.c_str());
                        }
                        tempGameObject->mTransform->setPosition(transformNums[0], transformNums[1], transformNums[2]);
                        tempGameObject->mTransform->setScale(transformNums[3], transformNums[4], transformNums[5]);
                        tempGameObject->mTransform->setRotation(transformNums[6], transformNums[7], transformNums[8]);
                    }
                }
                tempGameObject->setMaterial(tempMaterial);
                qDebug() << QString(tempGameObject->getGameobjectType()) << " was added through loading";
                gameobjects.push_back(tempGameObject);
            }
            else if(lineHeader == std::string("P"))
            {
                tempGameObject = new Plane(0.0, 0.0, 0.0, 10, 10, QString("Plane"));

                while(lineHeader != std::string("~"))
                {
                    std::stringstream sStreamLoop;
                    text = in.readLine().toStdString();
                    sStreamLoop << text;
                    sStreamLoop >> lineHeader;

                    if(lineHeader == std::string("N"))
                    {
                        sStreamLoop >> name;
                        tempGameObject->setName(QString::fromStdString(name));
                    }
                    else if(lineHeader == std::string("T"))
                    {
                        std::string transform;
                        for(int x = 0; x < 9; x++) // transform has 9 numbers, always
                        {
                            sStreamLoop >> transform;
                            transformNums[x] = atof(transform.c_str());
                        }
                        tempGameObject->mTransform->setPosition(transformNums[0], transformNums[1], transformNums[2]);
                        tempGameObject->mTransform->setScale(transformNums[3], transformNums[4], transformNums[5]);
                        tempGameObject->mTransform->setRotation(transformNums[6], transformNums[7], transformNums[8]);
                    }
                }
                tempGameObject->setMaterial(tempMaterial);
                qDebug() << QString(tempGameObject->getGameobjectType()) << " was added through loading";
                gameobjects.push_back(tempGameObject);
            }
            else if(lineHeader == std::string("I"))
            {
                tempGameObject = new Objloader(0.0f, 0.0f, 0.0f, " ");

                while(lineHeader != std::string("~"))
                {
                    std::stringstream sStreamLoop;
                    text = in.readLine().toStdString();
                    sStreamLoop << text;
                    sStreamLoop >> lineHeader;

                    if(lineHeader == std::string("N"))
                    {
                        sStreamLoop >> name;
                        tempGameObject->setName(QString::fromStdString(name));
                    }
                    else if(lineHeader == std::string("T"))
                    {
                        std::string transform;
                        for(int x = 0; x < 9; x++) // transform has 9 numbers, always
                        {
                            sStreamLoop >> transform;
                            transformNums[x] = atof(transform.c_str());
                        }
                        tempGameObject->mTransform->setPosition(transformNums[0], transformNums[1], transformNums[2]);
                        tempGameObject->mTransform->setScale(transformNums[3], transformNums[4], transformNums[5]);
                        tempGameObject->mTransform->setRotation(transformNums[6], transformNums[7], transformNums[8]);
                    }
                }
                tempGameObject->setMaterial(tempMaterial);
                qDebug() << QString(tempGameObject->getGameobjectType()) << " was added through loading";
                gameobjects.push_back(tempGameObject);
            }
            else if(lineHeader == std::string("B"))
            {
                tempGameObject = new OktaederBall(2);

                while(lineHeader != std::string("~"))
                {
                    std::stringstream sStreamLoop;
                    text = in.readLine().toStdString();
                    sStreamLoop << text;
                    sStreamLoop >> lineHeader;

                    if(lineHeader == std::string("N"))
                    {
                        sStreamLoop >> name;
                        tempGameObject->setName(QString::fromStdString(name));
                    }
                    else if(lineHeader == std::string("T"))
                    {
                        std::string transform;
                        for(int x = 0; x < 9; x++) // transform has 9 numbers, always
                        {
                            sStreamLoop >> transform;
                            transformNums[x] = atof(transform.c_str());
                        }
                        tempGameObject->mTransform->setPosition(transformNums[0], transformNums[1], transformNums[2]);
                        tempGameObject->mTransform->setScale(transformNums[3], transformNums[4], transformNums[5]);
                        tempGameObject->mTransform->setRotation(transformNums[6], transformNums[7], transformNums[8]);
                    }

                }
                tempGameObject->setMaterial(tempMaterial);
                qDebug() << QString(tempGameObject->getGameobjectType()) << " was added through loading";
                gameobjects.push_back(tempGameObject);
            }
            else if(lineHeader == std::string("S"))
            {
                tempGameObject = new SkyBox();

                while(lineHeader != std::string("~"))
                {
                    std::stringstream sStreamLoop;
                    text = in.readLine().toStdString();
                    sStreamLoop << text;
                    sStreamLoop >> lineHeader;

                    if(lineHeader == std::string("N"))
                    {
                        sStreamLoop >> name;
                        tempGameObject->setName(QString::fromStdString(name));
                    }
                    else if(lineHeader == std::string("T"))
                    {
                        std::string transform;
                        for(int x = 0; x < 9; x++) // transform has 9 numbers, always
                        {
                            sStreamLoop >> transform;
                            transformNums[x] = atof(transform.c_str());
                        }
                        tempGameObject->mTransform->setPosition(transformNums[0], transformNums[1], transformNums[2]);
                        tempGameObject->mTransform->setScale(transformNums[3], transformNums[4], transformNums[5]);
                        tempGameObject->mTransform->setRotation(transformNums[6], transformNums[7], transformNums[8]);
                    }

                }
                tempGameObject->setMaterial(tempMaterial);
                qDebug() << QString(tempGameObject->getName());
                gameobjects.push_back(tempGameObject);
            }
        }
    }

    tempGameObject = nullptr;
    mFile.close();
}

