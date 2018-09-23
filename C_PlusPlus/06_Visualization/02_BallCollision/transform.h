#ifndef TRANSFORM_H
#define TRANSFORM_H

#include <QVector3D>
#include "vec3.h"



//This script is used to transform objects in 3D-space.
//See GravityPhysics for the collision detection.

//forward declaration:
class GameObject;

class Transform
{
public:
    Transform(GameObject* owner);
	Vec3 getPosition();
	Vec3 getRotation();
	Vec3 getScale();

    void setPosition(float x, float y, float z);
	void setPosition(Vec3 pos) { mLocalPosition = pos; }

    void setRotation(float x, float y, float z);
	void setRotation(Vec3 rot) { mLocalRotation = rot; }

    void setScale(float x, float y, float z);
	void setScale(Vec3 scale) { mLocalScale = scale; }

    void translate(float x, float y, float z);
    void rotate(float x, float y, float z);
    //void rotate(float amount, QVector3D axis);

    void GravityPhysics(Vec3 newTranslate, bool collision);

    void ResetPhysics();

private:
	GameObject* mGameObject;

	Vec3 mLocalPosition;
	Vec3 mLocalRotation;
	Vec3 mLocalScale;

    GLfloat gravityScale = 9.81f;
    Vec3 currentTranslate;

    Vec3 velocity;
};

#endif // TRANSFORM_H
