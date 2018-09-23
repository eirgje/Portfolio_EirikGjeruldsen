#include "sosfile.h"
#include "vec3.h"
#include "vec2.h"

#include <QOpenGLExtraFunctions>
#include "constants.h"
#include <QFile>
#include <iostream>
#include <sstream>
#include <fstream>
#include <string>
#include <QVector4D>
#include <QMessageBox>


SosFile::SosFile(float xPos, float yPos, float zPos, QString name) : GameObject(xPos, yPos, zPos, name)
{
    initGeometry();
}

void SosFile::drawGeometry()
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

    // Draw cube geometry using indices from VBO 1
    glDrawElements(GL_POINTS, indices.size(), GL_UNSIGNED_SHORT, 0);



    //Unbind buffers:
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    //Unbind shader
    glUseProgram(0);
}

int SosFile::initGeometry()
{
    ReadSosi();
    initializeOpenGLFunctions();


    initializeOpenGLFunctions();
    glGenBuffers(1, &mVertexBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, mVertexBuffer);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), &vertices[0], GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &mIndexBuffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indices.size() * sizeof(GLint), &indices[0], GL_STATIC_DRAW);


    return 0;
}

void SosFile::setRendermode(int mode)
{
    if (mode == 1)
        mWireFrame = true;
    else if (mode == 0)
        mWireFrame = false;
}

void SosFile::setMaterial(Material *materialIn)
{
    mMaterial = materialIn;
}


void SosFile::ReadSosi()
{

    std::string file = Orf::assetFilePath.toStdString() + "Basisdata_1001_Kristiansand_25832_N50Hoyde_SOSI.sos";
    std::ifstream fileIn(file);
    if(!fileIn)
        qDebug() << "Could not open file for reading: " << QString::fromStdString(file);

    std::string oneLine, oneWord;

    std::vector<Vec3> tempVertices;

    Vec3 currentVector;

    int amountOfCurves = 1;

    GLint index = 0;

    int currentState = 0;

    bool first = true;
    Vec3 removeOffset;

    while(std::getline(fileIn, oneLine))
    {
        std::stringstream sStream;

        sStream << oneLine;
        sStream >> oneWord;


        if (oneWord == ".SLUTT"){
            indices.pop_back();
            fileIn.close();
            continue;
        }

        if (oneWord == "..H\xd8YDE")
        {
            sStream >> oneWord;
            currentVector.y = std::stof(oneWord);
            continue;
        }
        if ((oneWord == "..N\xd8" || oneWord == "..N\xd8H") && currentState == 0)
        {
            currentState++;
        }
        else if(currentState == 1){

            if(oneWord == ".KURVE" || oneWord == ".PUNKT" ){
                currentState = 0;
                amountOfCurves++;
                indices.pop_back();
                indices.pop_back();
                continue;
            }
            if(oneWord == "..N\xd8" || oneWord == "..N\xd8H"){
                qDebug() << "Random Nø appeared";
                continue;
            }

            currentVector.x = std::stof(oneWord);

            sStream >> oneWord;

            currentVector.z = std::stof(oneWord);

            currentVector = Vec3(currentVector.x / 10000, currentVector.y / 100, currentVector.z  / 10000);

            if (first)
            {
                removeOffset = currentVector;
                first = false;
            }
            currentVector.x -= removeOffset.x;
            currentVector.z -= removeOffset.z;

            indices.push_back(index);
            indices.push_back(index + 1);

            tempVertices.push_back(currentVector);


            //qDebug() << "Draw line between " << QString::number(index) << " + " <<  QString::number(index + 1);

            index++;


        }
        continue;
    }

    //We now have to implement this into vertices and indices.


    //Adding vertex to the vector-list 'vetices'
    for (uint i = 0; i < tempVertices.size(); i++){
        Vertex tempVertex = {tempVertices[i], Vec3(0.0f, 0.0f,  0.0f), Vec2(0.0f, 0.0f)};
        tempVertex.set_rgb(i /10, i / 10, i / 10);
        vertices.push_back(tempVertex);
    }
}





