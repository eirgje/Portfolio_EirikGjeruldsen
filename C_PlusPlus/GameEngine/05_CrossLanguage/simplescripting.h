#ifndef SIMPLESCRIPTING_H
#define SIMPLESCRIPTING_H

#include <QObject>
#include "vec3.h"
#include <qfile.h>
#include "qjsengine.h"


//This code is to allow the user of the software to program using JavaScript.
//Also giving some functionality from what has already been written in C++.


class GameObject;

class ScriptFunctions : public QObject
{
    Q_OBJECT
public:
    ScriptFunctions(QObject *parent = 0);

    Q_INVOKABLE void SetPosition(float x, float y, float z);

    void SetGameObject(GameObject *newGameObject){mGameobject = newGameObject;}

    GameObject *mGameobject = nullptr;


};

class SimpleScripting
{
public:

    SimpleScripting(QString filepath = "", GameObject *tempGameobject = nullptr);

    void moveBall();

private:
    QString filename = "";
    QFile *file = nullptr;
    QJSEngine mEngine;

    ScriptFunctions mScriptingFunction;


};


#endif // SIMPLESCRIPTING_H
