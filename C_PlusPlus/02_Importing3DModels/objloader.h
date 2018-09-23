#ifndef OBJLOADER_H
#define OBJLOADER_H

#include "vector"
#include "vec2.h"
#include "vec3.h"
#include "gameobject.h"
#include "vertex.h"
#include "material.h"


//This class is used to read and OBJ-file.
//OBJ-files are files exported from a 3D software, but contains less information for shader use than an FBX-file.
//There are three different type of models imported for each model, these are used for the purpose of Level-Of-Detail found in lod.h and lod.cpp.

class Objloader : public GameObject
{
public:
    Objloader(float x, float y, float z, QString name);
    void drawGeometry();
    int initGeometry();
    void setRendermode(int mode);

    void setMaterial(Material *materialIn);
    void SetVerticesCount(std::vector<Vertex> listRef);


private:
    GLuint mVertexBuffer;
    GLuint mIndexBuffer;

    GLuint vertexBuffers[2];
    GLuint indexBuffers[2];

    GLuint mNumOfVertices;
    Material *mMaterial;

    QMatrix4x4 mMVPMatrix;
    void loadOBJ(std::string fileName);

    std::vector<Vertex> mVertices;
    std::vector<GLushort> indicesCount;

    std::vector<Vertex> verticesLOD0;
    std::vector<Vertex> verticesLOD1;
    std::vector<Vertex> verticesLOD2;

    std::vector<GLint> indicesLOD0;
    std::vector<GLushort> indicesLOD1;
    std::vector<GLushort> indicesLOD2;

    int currentRecurison = 0;

    int indexSizeGV = 0;





};

#endif // OBJLOADER_H
