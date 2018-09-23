#ifndef GAMEOBJECT_H
#define GAMEOBJECT_H

#include <QOpenGLFunctions>
#include <QOpenGLShaderProgram>
#include <QOpenGLBuffer>
#include <QMatrix4x4>
#include "ballcollision.h"
#include "material.h"
#include "vertex.h"


//Check CollisionDetection to see how the collision is calculated.
//This function is used in the transform class.

class GameObject : protected QOpenGLFunctions
{
public:
	GameObject(float xPos = 0.0, float yPos = 0.0, float zPos = 0.0, QString nameIn = "GameObject");
    virtual ~GameObject();

    virtual void drawGeometry() = 0;
    virtual void setMaterial(Material *materialIn) = 0;
    virtual void setRendermode(int mode) = 0;

    void setViewMatrix(QMatrix4x4 viewMatrix);
    void setprojectionMatrix(QMatrix4x4 projectionMatrix);
	void setName(QString nameIn);

	QString getName();

    QMatrix4x4 getMatrix();

	class Transform *mTransform;

    Vec3 CollisionDetection(Vec3 positionIn);

	GameObject* getParent() { return mParent; }
    void setParent(GameObject *parent);

    GLfloat GetRadius();
    GLfloat GetCurrentY();

    bool GetCollisionState();


protected:
    virtual int initGeometry() = 0;

	QMatrix4x4 mModelMatrix;
	QMatrix4x4 mViewMatrix;
	QMatrix4x4 mProjectionMatrix;

	GameObject* mParent;
	QString name;

	bool mWireFrame = false;

    GLfloat radius = 0;

    //Collision

    std::vector<Vertex> vertList;
    Vec3 p1, p2, p3;
    bool colided = false;
    GLfloat currentY = 0.0f;

private:
    bool firstTriangleFound = false;

    int pointOne, pointTwo, pointThree;

    Vec3 savedRBG1, savedRBG2, savedRBG3;
};

#endif // GAMEOBJECT_H
