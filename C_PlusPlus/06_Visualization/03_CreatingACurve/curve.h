#ifndef CURVE_H
#define CURVE_H

#include "gameobject.h"
#include "vec3.h"


//This code will draw a bezier curve using 4 xyz points.

class Curve : public GameObject
{
public:
    Curve(GameObject *p1, GameObject *p2, GameObject *p3, GameObject *p4, bool spiral);
    void drawGeometry();
    int initGeometry();

    int initGeometryAfterStart();

    void setRendermode(int mode);

    void setMaterial(Material *materialIn);

protected:
    Vec3 Bezier(int degree, float t);
    void Spiral();

private:
    GLuint mVertexBuffer;
    GLuint mIndexBuffer;

    Material *mMaterial;

    std::vector <Vertex> vertices;
    std::vector <GLshort> indices;

    QMatrix4x4 mMVPMatrix;
    Vec3 controlPoints[4];

    GameObject *gp1, *gp2, *gp3, *gp4;

    bool makingSpiral = false;
};

#endif // CURVE_H
