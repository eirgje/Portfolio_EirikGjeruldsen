#include "simplescripting.h"

#include <QDebug>

#include "gameobject.h"
#include "transform.h"

SimpleScripting::SimpleScripting(QString filepath, GameObject *tempGameobject)
{
    mScriptingFunction.SetGameObject(tempGameobject);

    filename = filepath;

    file = new QFile(filepath);

    if(!file->open(QIODevice::ReadOnly))
        qDebug()<< "No file found at: " << file->fileName();

    QTextStream stream(&*file);
    QString content = stream.readAll();

    file->close();

    QJSValue value = mEngine.newQObject(&mScriptingFunction);

    mEngine.evaluate(content, file->fileName());

    mEngine.globalObject().setProperty("GameObject", value);
}


void SimpleScripting::moveBall()
{
    if(file == nullptr)
        return;

    QJSValue functionCall = mEngine.evaluate("Move");

    functionCall.call();
}

void ScriptFunctions::SetPosition(float x, float y, float z)
{
    mGameobject->mTransform->rotate(x,y,z);
}



ScriptFunctions::ScriptFunctions(QObject *parent) : QObject(parent)
{

}
