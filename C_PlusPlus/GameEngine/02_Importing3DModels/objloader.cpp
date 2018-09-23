#include "objloader.h"
#include <QOpenGLExtraFunctions>
#include "constants.h"
#include <QFile>
#include <iostream>
#include <sstream>
#include <fstream>
#include <string>

#include <QVector4D>

Objloader::Objloader(float xPos, float yPos, float zPos, QString name) : GameObject (xPos, yPos, zPos, name, QString("Import"))
{
    initGeometry();
}


void Objloader::drawGeometry()
{


    glBindBuffer(GL_ARRAY_BUFFER, vertexBuffers[LODstate]);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffers[LODstate]);


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


    if(LODstate == 0){
        SetVerticesCount(verticesLOD0);
        // Draw object geometry using indices from VBO 1
        if (!mWireFrame)
            glDrawElements(GL_TRIANGLES, indicesLOD0.size(), GL_UNSIGNED_SHORT, 0);
        else
            glDrawElements(GL_LINES, indicesLOD0.size(), GL_UNSIGNED_SHORT, 0);
    } else if(LODstate == 1){
        SetVerticesCount(verticesLOD1);
        // Draw object geometry using indices from VBO 1
        if (!mWireFrame)
            glDrawElements(GL_TRIANGLES, indicesLOD1.size(), GL_UNSIGNED_SHORT, 0);
        else
            glDrawElements(GL_LINES, indicesLOD1.size(), GL_UNSIGNED_SHORT, 0);
    } else if(LODstate == 2){
        SetVerticesCount(verticesLOD2);
        // Draw object geometry using indices from VBO 1
        if (!mWireFrame)
            glDrawElements(GL_TRIANGLES, indicesLOD2.size(), GL_UNSIGNED_SHORT, 0);
        else
            glDrawElements(GL_LINES, indicesLOD2.size(), GL_UNSIGNED_SHORT, 0);
    } else {
        qDebug() << "There is no such LOD level.";
    }


    //Unbind buffers:
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

    //Unbind shader
    glUseProgram(0);

}

int Objloader::initGeometry()
{
    while (currentRecurison<3){
        if ( currentRecurison == 0)
            loadOBJ("Suzanne.obj.txt");
        else if(currentRecurison == 1)
            loadOBJ("Suzanne_L01.obj.txt");
        else if(currentRecurison == 2)
            loadOBJ("Suzanne_L02.obj.txt");
        else
            qDebug() << "To many recursions happen";
        currentRecurison++;
    }



    initializeOpenGLFunctions();

    //--------LOD0------------------
    glGenBuffers(1, &vertexBuffers[0]);
    glBindBuffer(GL_ARRAY_BUFFER, vertexBuffers[0]);
    glBufferData(GL_ARRAY_BUFFER, verticesLOD0.size() * sizeof(Vertex), &verticesLOD0[0], GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &indexBuffers[0]);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffers[0]);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indicesLOD0.size()*sizeof(GLushort), &indicesLOD0[0], GL_STATIC_DRAW);




    //--------LOD1------------------
    glGenBuffers(1, &vertexBuffers[1]);
    glBindBuffer(GL_ARRAY_BUFFER, vertexBuffers[1]);
    glBufferData(GL_ARRAY_BUFFER, verticesLOD1.size() * sizeof(Vertex), &verticesLOD1[0], GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &indexBuffers[1]);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffers[1]);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indicesLOD1.size()*sizeof(GLushort), &indicesLOD1[0], GL_STATIC_DRAW);




    //--------LOD2------------------
    glGenBuffers(1, &vertexBuffers[2]);
    glBindBuffer(GL_ARRAY_BUFFER, vertexBuffers[2]);
    glBufferData(GL_ARRAY_BUFFER, verticesLOD2.size() * sizeof(Vertex), &verticesLOD2[0], GL_STATIC_DRAW);

    // Transfer index data to VBO 1
    glGenBuffers(1, &indexBuffers[2]);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffers[2]);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, indicesLOD2.size()*sizeof(GLushort), &indicesLOD2[0], GL_STATIC_DRAW);


    return 0;

}

void Objloader::setRendermode(int mode)
{
    if (mode == 1)
        mWireFrame = true;
    else if (mode == 0)
        mWireFrame = false;
}

void Objloader::setMaterial(Material *materialIn)
{
    mMaterial = materialIn;
}

void Objloader::SetVerticesCount(std::vector<Vertex> listRef)
{
    vertAmount = 0;
    for(uint i = 0; i < listRef.size(); i++){
        vertAmount++;
    }
}



void Objloader::loadOBJ(std::string fileName)
{
    //Due to limited time and bottlenecking with the OBJ import I chose to base this on OleFlatens code.
    //My try is a the bottom of this function.

    //Open File
    std::string file = Orf::assetFilePath.toStdString() + fileName;
    std::ifstream fileIn(file);
    if(!fileIn)
        qDebug() << "Could not open file for reading: " << QString::fromStdString(file);

    //One line at a time-variable
    std::string oneLine;
    //One word at a time-variable
    std::string oneWord;

    std::vector<Vec3> tempVertices;
    std::vector<Vec3> tempNormals;
    std::vector<Vec2> tempUVs;

    std::vector<Vertex> verticesVector;
    std::vector<GLushort> indicesVector;

    // Varible for constructing the indices vector
    int temp_index = 0;

    //Reading one line at a time from file to oneLine
    while(std::getline(fileIn, oneLine))
    {
        //Doing a trick to get one word at a time
        std::stringstream sStream;
        //Pushing line into stream
        sStream << oneLine;
        //Streaming one word out of line
        sStream >> oneWord;

        if (oneWord == "#")
        {
            //Ignore this line
            //qDebug() << "Line is comment "  << QString::fromStdString(oneWord) << endl;
            continue;
        }
        if (oneWord == "v")
        {
            //qDebug() << "Line is vertex "  << QString::fromStdString(oneWord) << " ";
            Vec3 tempVertex;
            sStream >> oneWord;
            tempVertex.x = std::stof(oneWord);
            sStream >> oneWord;
            tempVertex.y = std::stof(oneWord);
            sStream >> oneWord;
            tempVertex.z = std::stof(oneWord);

            //Vertex made - pushing it into vertex-vector
            tempVertices.push_back(tempVertex);

            continue;
        }
        if (oneWord == "vt")
        {
            //qDebug() << "Line is UV-coordinate "  << QString::fromStdString(oneWord) << " ";
            Vec2 tempUV;
            sStream >> oneWord;
            tempUV.x = std::stof(oneWord);
            sStream >> oneWord;
            tempUV.y = std::stof(oneWord);

            //UV made - pushing it into UV-vector
            tempUVs.push_back(tempUV);

            continue;
        }
        if (oneWord == "vn")
        {
            //qDebug() << "Line is normal "  << QString::fromStdString(oneWord) << " ";
            Vec3 tempNormal;
            sStream >> oneWord;
            tempNormal.x = std::stof(oneWord);
            sStream >> oneWord;
            tempNormal.y = std::stof(oneWord);
            sStream >> oneWord;
            tempNormal.z = std::stof(oneWord);

            //Vertex made - pushing it into vertex-vector
            tempNormals.push_back(tempNormal);
            continue;
        }
        if (oneWord == "f")
        {
            //qDebug() << "Line is a face "  << QString::fromStdString(oneWord) << " ";
            //int slash; //used to get the / from the v/t/n - format
            int index, normal, uv;
            for(int i = 0; i < 3; i++)
            {
                sStream >> oneWord;     //one word read
                std::stringstream tempWord(oneWord);    //to use getline on this one word
                std::string segment;    //the numbers in the f-line
                std::vector<std::string> segmentArray;  //temp array of the numbers
                while(std::getline(tempWord, segment, '/')) //splitting word in segments
                {
                    segmentArray.push_back(segment);
                }
                index = std::stoi(segmentArray[0]);     //first is vertex
                if (segmentArray[1] != "")              //second is uv
                    uv = std::stoi(segmentArray[1]);
                else
                {
                    //qDebug() << "No uvs in mesh";       //uv not present
                    uv = 0;                             //this will become -1 in a couple of lines
                }
                normal = std::stoi(segmentArray[2]);    //third is normal

                //Fixing the indexes
                //because obj f-lines starts with 1, not 0
                --index;
                --uv;
                --normal;

                if (uv > -1)    //uv present!
                {
                    Vertex tempVert(tempVertices[index], tempNormals[normal], tempUVs[uv]);
                    verticesVector.push_back(tempVert);
                }
                else            //no uv in mesh data, use 0, 0 as uv
                {
                    Vertex tempVert(tempVertices[index], tempNormals[normal], Vec2(0.0f, 0.0f));
                    verticesVector.push_back(tempVert);
                }
                indicesVector.push_back(temp_index++);
            }
            continue;
        }

        //if we get down here, the line is something we don't care about.
        //qDebug() << "Line was something strange! " << QString::fromStdString(oneWord);
    }
    //beeing a nice boy and closing the file after use
    fileIn.close();

    int vertexSize = verticesVector.size();
    int indexSize = indicesVector.size();
    indexSizeGV = indexSize;

    //Making general arrays for the vertecies and indices
    Vertex vertices[vertexSize];
    GLushort indices[indexSize];

    for (int counter{0}; counter < vertexSize; ++counter)
    {
        vertices[counter] = verticesVector[counter];
    }

    for (int counter{0}; counter < indexSize; ++counter)
    {
        indices[counter] = indicesVector[counter];
    }

    if(currentRecurison == 0){

        for(int i = 0; i < vertexSize; i++){
            Vertex newTempVert = vertices[i];
            verticesLOD0.push_back(newTempVert);
        }

        for(int i = 0; i < indexSize; i++){
            GLushort newGLshort = indices[i];
            indicesLOD0.push_back(newGLshort);
        }
    }
    else if(currentRecurison == 1){
        for(int i = 0; i < vertexSize; i++){
            Vertex newTempVert = vertices[i];
            verticesLOD1.push_back(newTempVert);
        }

        for(int i = 0; i < indexSize; i++){
            GLushort newGLshort = indices[i];
            indicesLOD1.push_back(newGLshort);
        }
    }
    else if(currentRecurison == 2){
        for(int i = 0; i < vertexSize; i++){
            Vertex newTempVert = vertices[i];
            verticesLOD2.push_back(newTempVert);
        }

        for(int i = 0; i < indexSize; i++){
            GLushort newGLshort = indices[i];
            indicesLOD2.push_back(newGLshort);
        }
    }
}

