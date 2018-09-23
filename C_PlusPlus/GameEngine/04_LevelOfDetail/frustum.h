#ifndef FRUSTUM_H
#define FRUSTUM_H


#include <QVector4D>
#include <QMatrix4x4>
#include "transform.h"
#include "gameobject.h"
#include "vertex.h"
#include "material.h"
#include "camera.h"

//This creates boundaries for the camera.
//These boundaries are used to check if you should render a 3D object or not.
//Mostly used to reduce stress on the GPU.

class Frustum : public GameObject
{
public:
    Frustum(Camera *tempCamera);

    void drawGeometry();
    int initGeometry();

    void setMaterial(Material *materialIn);

    void setRendermode(int mode){qDebug() << mode;}

    QString GetType() { return type; }

private:
    GLuint mVertexBuffer;
    GLuint mIndexBuffer;

    Camera *mCamera = nullptr;


    Material *mMaterial;

    QMatrix4x4 mMVPMatrix;
};

#endif // FRUSTUM_H + 1
