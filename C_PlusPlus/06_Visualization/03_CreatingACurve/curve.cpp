
#include "curve.h"

#include <tgmath.h>

Curve::Curve(GameObject *g1 = nullptr, GameObject *g2 = nullptr, GameObject *g3 = nullptr, GameObject *g4 = nullptr, bool spiral = false) : GameObject(0.0f, 0.0f, 0.0f, QString("Cruve"))
{
    gp1 = g1;
    gp2 = g2;
    gp3 = g3;
    gp4 = g4;
    makingSpiral = spiral;
    initGeometry();
}


void Curve::drawGeometry()
{
    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
    mMaterial->useMaterial();

    //refresh modelMatrix:
    getMatrix();

    mMVPMatrix = mProjectionMatrix*mViewMatrix*mModelMatrix;
    mMaterial->setMVPMatrix(mMVPMatrix);

    // Offset for position
    // Positions are the first data, therefor offset is 0
    int offset = 0;

    // Tell OpenGL programmable pipeline how to locate vertex position data
    glVertexAttribPointer(static_cast<GLuint>(mMaterial->getPositionAttribute()), 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<const void*>(offset));

    // Offset for vertex coordinates
    // before normals
    offset += sizeof(Vec3);

    glVertexAttribPointer(static_cast<GLuint>(mMaterial->getNormalAttribute()), 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<const void*>(offset));

    // Offset for normal coordinates
    // before UVs
    offset += sizeof(Vec3);

    // Tell OpenGL programmable pipeline how to locate vertex texture coordinate data
    glVertexAttribPointer(static_cast<GLuint>(mMaterial->getTextureAttribute()), 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), reinterpret_cast<const void*>(offset));

    //glDrawArrays(GL_LINE_STRIP, 1, vertices.size());

    glDrawElements(GL_LINES, indices.size(), GL_UNSIGNED_SHORT, 0);


    //Unbind buffers:
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    //Unbind shader
    glUseProgram(0);
}

int Curve::initGeometry()
{

    controlPoints[0] = Vec3(0.0f, 0.0f, 0.0f);
    controlPoints[1] = Vec3(0.0f, 2.0f, 0.0f);
    controlPoints[2] = Vec3(3.0f, 2.0f, 0.0f);
    controlPoints[3] = Vec3(3.0f, 1.0f, 0.0f);

    gp1->mTransform->setPosition(controlPoints[0]);
    gp2->mTransform->setPosition(controlPoints[1]);
    gp3->mTransform->setPosition(controlPoints[2]);
    gp4->mTransform->setPosition(controlPoints[3]);

    if(!makingSpiral){
        for (int i = 0; i < 100; i++){
            Vertex tempVert;
            tempVert.set_xyz(Bezier(3, i * 0.01f));
            tempVert.set_normal(255.0f,0.0f,0.0f);
            vertices.push_back(tempVert);
            indices.push_back(i);
            indices.push_back(i+1);
        }
    } else {
        Spiral();
    }

    indices.pop_back();
    indices.pop_back();

    qDebug() << "First curve: " << QString::number(vertices.size());
    qDebug() << "Indices " << QString::number(indices.size()) << " | Vertices " << QString::number(vertices.size());

    for (int i = 0; i < indices.size(); i++){
        qDebug() << "Index: " << QString::number(i) << "Indices " << QString::number(indices[i]);
    }


    initializeOpenGLFunctions();

    // Transfer vertex data to VBO 0
    glGenBuffers(1, &mVertexBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBufferData(GL_ARRAY_BUFFER, vertices.size()*sizeof(Vertex), &vertices[0], GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &mIndexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size()*sizeof(GLushort), &indices[0], GL_STATIC_DRAW);

    return 0;
}

int Curve::initGeometryAfterStart()
{
    int i = 0;
    while(i != indices.size()){
        indices.pop_back();

    }
    while(i != vertices.size()){
        vertices.pop_back();

    }

    controlPoints[0] = gp1->mTransform->getPosition();
    controlPoints[1] = gp2->mTransform->getPosition();
    controlPoints[2] = gp3->mTransform->getPosition();
    controlPoints[3] = gp4->mTransform->getPosition();
    qDebug() << QString::number(controlPoints[0].y);
    qDebug() << QString::number(controlPoints[1].y);
    qDebug() << QString::number(controlPoints[2].y);
    qDebug() << QString::number(controlPoints[3].y);



    if(!makingSpiral){
        for (int i = 0; i < 100; i++){
            Vertex tempVert;
            tempVert.set_xyz(Bezier(3, i * 0.01f));
            tempVert.set_normal(255.0f,0.0f,0.0f);
            vertices.push_back(tempVert);
            indices.push_back(i);
            indices.push_back(i+1);
        }
    } else {
        Spiral();
    }


    indices.pop_back();
    indices.pop_back();


    initializeOpenGLFunctions();

    // Transfer vertex data to VBO 0
    glGenBuffers(1, &mVertexBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBufferData(GL_ARRAY_BUFFER, vertices.size()*sizeof(Vertex), &vertices[0], GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &mIndexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size()*sizeof(GLushort), &indices[0], GL_STATIC_DRAW);

    return 0;
}

void Curve::setRendermode(int mode)
{
    if (mode == 1)
        mWireFrame = true;
    else if (mode == 0)
        mWireFrame = false;
}

void Curve::setMaterial(Material *materialIn)
{
    mMaterial = materialIn;
}

Vec3 Curve::Bezier(int degree, float t)
{
    Vec3 c[4];
    for (int i=0; i<4; i++)
        c[i] = controlPoints[i];
    for (int k=1; k<=degree; k++)
    {
        for (int i=0; i<degree-k+1; i++)
            c[i] = c[i] * (1-t) + c[i+1] * t;
    }
    return c[0];
}

void Curve::Spiral()
{
    // spiral: x = (t) = cos t, y(t) = sin t, z = t

    for (int i = 0; i < 100; i++){

        Vec3 currentVector = Vec3(cos(i * 0.1f), sin(i * 0.1f), i * 0.1f);
        Vertex tempVert;
        tempVert.set_xyz(currentVector);
        tempVert.set_normal(124.0f,252.0f,0.0f);
        vertices.push_back(tempVert);
        indices.push_back(i);
        indices.push_back(i+1);
    }
}
