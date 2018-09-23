#include "frustum.h"

Frustum::Frustum(Camera *tempCamera = nullptr)
{
    type = QString("Camera");
    mCamera = tempCamera;
    initGeometry();
}


void Frustum::drawGeometry()
{
    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
    mMaterial->useMaterial();

    //refresh modelMatrix:
    getMatrix();

    mMVPMatrix = mProjectionMatrix * mViewMatrix * mModelMatrix;
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

    glDrawElements(GL_LINE_STRIP, 16, GL_UNSIGNED_SHORT, 0);


    //Unbind buffers:
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    //Unbind shader
    glUseProgram(0);
}

int Frustum::initGeometry()
{

//    QMatrix4x4 prespectiveMatrix = prespectiveMatrix().inverted();

//    float min = -1.0f;
//    float max = 1.0f;


//    Vertex vertices[] = {
//        {Vec3(max, min, min), color, Vec2()}, //down left near
//        {Vec3(min, min, min), color, Vec2()}, //down right near
//        {Vec3(min, max, min), color, Vec2()}, //up right near
//        {Vec3(max, max, min), color, Vec2()}, //up left near
//        {Vec3(max, min, max), color, Vec2()}, //down left far
//        {Vec3(min, min, max), color, Vec2()}, //down right far
//        {Vec3(min, max, max), color, Vec2()}, //up right far
//        {Vec3(max, max, max), color, Vec2()} //up left far
//    };



    Vec3 color(0.0f, 1.0f, 0.0f);
    Vec2 uv = Vec2(0,0);



    Vertex vertices[] = {
        {Vec3(-1.0f, -1.0f, 1.0f), color, uv}, //down left near
        {Vec3(1.0f, -1.0f, 1.0f), color, uv}, //down right near
        {Vec3(1.0f, 1.0f, 1.0f), color, uv}, //up right near
        {Vec3(-1.0f, 1.0f, 1.0f), color, uv}, //up left near
        {Vec3(-1.0f, -1.0f, -1.0f), color, uv}, //down left far
        {Vec3(1.0f, -1.0f, -1.0f), color, uv}, //down right far
        {Vec3(1.0f, 1.0f, -1.0f), color, uv}, //up right far
        {Vec3(-1.0f, 1.0f, -1.0f), color, uv} //up left far
    };

    //Using GL_LINE_STRIP
    GLushort indices[] = {
        0, 1, 2, 3, 7, 6, 5, 4, 0,
        3, 7, 4, 5, 1, 2, 6
    };
    QMatrix4x4 inverseP = mCamera->getPerspectiveMatrix().inverted();
   // QMatrix4x4 inverseV = mCamera->getMatrix().inverted();

    //Frustum needs to be moved to initial position of camera
    Vec3 initialPosition = mCamera->mTransform->getPosition();

    for (int i{0}; i < 8; ++i)
    {
        QVector4D tempVert(vertices[i].get_x(), vertices[i].get_y(), vertices[i].get_z(), 1.0f);
        QVector4D vertexTransposed = inverseP * tempVert;
        vertexTransposed = vertexTransposed / vertexTransposed.w();
        //Add the initial position of the camera!
        vertices[i].set_xyz(vertexTransposed.x()+initialPosition.x, vertexTransposed.y()+initialPosition.y, vertexTransposed.z()+initialPosition.z);
    }

    Vec3 v0 = vertices[0].get_xyz();
    Vec3 v1 = vertices[1].get_xyz();
    Vec3 v2 = vertices[2].get_xyz();
    Vec3 v3 = vertices[3].get_xyz();
    Vec3 v4 = vertices[4].get_xyz();
    Vec3 v5 = vertices[5].get_xyz();
    Vec3 v6 = vertices[6].get_xyz();
    Vec3 v7 = vertices[7].get_xyz();

    mCamera->SetFrustumPoints(v0);
    mCamera->SetFrustumPoints(v1);
    mCamera->SetFrustumPoints(v2);
    mCamera->SetFrustumPoints(v3);
    mCamera->SetFrustumPoints(v4);
    mCamera->SetFrustumPoints(v5);
    mCamera->SetFrustumPoints(v6);
    mCamera->SetFrustumPoints(v7);



    //near plane
    Vec3 normalNear = ((v3 - v0) ^ (v1 - v0)).normalized();

    //Far plane
    Vec3 normalFar = ((v4 - v7) ^ (v6 - v7)).normalized();

    //Right plane
    Vec3 normalRight = ((v3 - v7) ^ (v4 - v7)).normalized();

    //Left plane
    Vec3 normalLeft = ((v5 - v6) ^ (v2 - v7)).normalized();

    //Top plane
    Vec3 normalTop = ((v6 - v7) ^ (v2 - v6)).normalized();

    //Bottom plane
    Vec3 normalBottom = ((v4 - v5) ^ (v1 - v5)).normalized();


    mCamera->SetFrustumNormals(normalNear);
    mCamera->SetFrustumNormals(normalFar);
    mCamera->SetFrustumNormals(normalRight);
    mCamera->SetFrustumNormals(normalLeft);
    mCamera->SetFrustumNormals(normalTop);
    mCamera->SetFrustumNormals(normalBottom);


    initializeOpenGLFunctions();

    // Transfer vertex data to VBO 0
    glGenBuffers(1, &mVertexBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBufferData(GL_ARRAY_BUFFER, 8*sizeof(Vertex), vertices, GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &mIndexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, 16*sizeof(GLushort), indices, GL_STATIC_DRAW);

    return 0;
}

void Frustum::setMaterial(Material *materialIn)
{
    mMaterial = materialIn;
}
