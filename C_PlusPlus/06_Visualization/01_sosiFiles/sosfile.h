#ifndef SOSFILE_H
#define SOSFILE_H


#include "gameobject.h"
#include "vertex.h"
#include "material.h"


//This software reads a SOSI-file (Norwegian mountain map).
//After reading it will create the moutain using dots consisting of xyz-positions.
//A sosi-file is added for reprensentation purposes.

class SosFile : public GameObject
{
public:
    SosFile(float xPos, float yPos, float zPos, QString name);
    void ReadSosi();

    void drawGeometry();
    int initGeometry();
    void setRendermode(int mode);

    void setMaterial(Material *materialIn);

private:
    GLuint mVertexBuffer;
    GLuint mIndexBuffer;

    Material *mMaterial;

    QMatrix4x4 mMVPMatrix;


    std::vector<Vertex> vertices;
    std::vector<GLint> indices;


};

#endif // SOSFILE_H
