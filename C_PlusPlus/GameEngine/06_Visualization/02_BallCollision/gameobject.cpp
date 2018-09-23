#include "gameobject.h"
#include "vec3.h"
#include "transform.h"

#include <QVector2D>
#include <QVector3D>

GameObject::GameObject(float xPos, float yPos, float zPos, QString nameIn) :mParent(nullptr)
{
    initializeOpenGLFunctions();

    mTransform = new Transform(this);
    mTransform->setPosition(Vec3(xPos, yPos, zPos));
    name = nameIn;
}

GameObject::~GameObject()
{
    delete mTransform;
}

void GameObject::setParent(GameObject *parent)
{
    mParent = parent;

    //Do something to my own transform?
}

GLfloat GameObject::GetRadius()
{
    return radius;
}

GLfloat GameObject::GetCurrentY()
{
    return currentY;
}

bool GameObject::GetCollisionState()
{
    return colided;
}

void GameObject::setViewMatrix(QMatrix4x4 viewMatrix)
{
    mViewMatrix = viewMatrix;
}

void GameObject::setprojectionMatrix(QMatrix4x4 projectionMatrix)
{
    mProjectionMatrix = projectionMatrix;
}

void GameObject::setName(QString nameIn)
{
    name = nameIn;
}

QString GameObject::getName()
{
    return name;
}

QMatrix4x4 GameObject::getMatrix()
{
    mModelMatrix.setToIdentity();
    mModelMatrix.translate(mTransform->getPosition().x, mTransform->getPosition().y, mTransform->getPosition().z );
    mModelMatrix.rotate(mTransform->getRotation().x, 1.0, 0.0, 0.0);
    mModelMatrix.rotate(mTransform->getRotation().y, 0.0, 1.0, 0.0);
    mModelMatrix.rotate(mTransform->getRotation().z, 0.0, 0.0, 1.0);
    mModelMatrix.scale(mTransform->getScale().x, mTransform->getScale().y, mTransform->getScale().z);
    //mModelMatrix.translate(mTransform.mPosition);

    return mModelMatrix;
}

Vec3 GameObject::CollisionDetection(Vec3 positionIn)
{

    GLint firstPoint = 0;
    GLint secondPoint = 1;
    GLint thirdPoint = 2;

    GLfloat u, v, w;


    for (unsigned int rounds = 0; rounds < vertList.size()/3; rounds++){

        p1 = vertList[firstPoint].get_xyz() + mTransform->getPosition();
        p2 = vertList[secondPoint].get_xyz() + mTransform->getPosition();
        p3 = vertList[thirdPoint].get_xyz() + mTransform->getPosition();

//        qDebug() << "1x: " << QString::number(vertList[firstPoint].get_x())
//                 << "1y " << QString::number(vertList[firstPoint].get_y())
//                 << "1z " << QString::number(vertList[firstPoint].get_z());


        Vec3 normalVector = (p2 - p1) ^ (p3 - p1);

        Vec3 directionalVector = p1 - positionIn;

        Vec3 centerPartVector = normalVector * ((directionalVector * normalVector) / (normalVector * normalVector));

        Vec3 projectionPoint = positionIn + centerPartVector;

        //qDebug() << QString::number(normalVector.x) << QString::number(normalVector.y) << QString::number(normalVector.z);





        //First get the areal(T)
        GLfloat areaT = (
                    (   (p1.x - p3.x)
                        *
                        (p2.y - p1.y)
                        -
                        (p3.y - p1.y)
                        *
                        (p1.x - p2.x)
                        ))/2;

        //Creating the barysentric coordinate variables.

        Vec3 pP1 = p1 - projectionPoint;
        Vec3 pP2 = p2 - projectionPoint;
        Vec3 pP3 = p3 - projectionPoint;

        u = (pP2.x * pP3.y - pP2.y * pP3.x) / areaT;
        v = (pP3.x * pP1.y - pP3.y * pP1.x) / areaT;
        w = (pP1.x * pP2.y - pP1.y * pP2.x) / areaT;



        if (u >= 0 || v >= 0 || w >= 0){

            float distance = (projectionPoint - positionIn).length();
            if(distance <= 1){
                if (firstTriangleFound){
                    vertList[pointOne].set_rgb(savedRBG1.x, savedRBG1.y, savedRBG1.z);
                    vertList[pointTwo].set_rgb(savedRBG2.x, savedRBG2.y, savedRBG2.z);
                    vertList[pointThree].set_rgb(savedRBG3.x, savedRBG3.y, savedRBG3.z);
                }

                pointOne = firstPoint;
                pointTwo = secondPoint;
                pointThree = thirdPoint;

                savedRBG1 = vertList[pointOne].get_rgb();
                savedRBG2 = vertList[pointTwo].get_rgb();
                savedRBG3 = vertList[pointThree].get_rgb();

                vertList[pointOne].set_rgb(255,255,150);
                vertList[pointTwo].set_rgb(255,255,150);
                vertList[pointThree].set_rgb(255,255,150);

//                qDebug() << "x: " << QString::number(vertList[pointOne].get_x())
//                         << "y " << QString::number(vertList[pointOne].get_y())
//                         << "z " << QString::number(vertList[pointOne].get_z());

                if (!firstTriangleFound)
                    firstTriangleFound = true;

                colided = true;
                return Vec3(centerPartVector).normalized();
            }
            colided = false;

        }
        firstPoint +=3;
        secondPoint +=3;
        thirdPoint +=3;
    }
    return Vec3(0.0f, 0.0f, 0.0f);
    colided = false;

}
