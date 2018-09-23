#include "gameengine.h"
#include "constants.h"
#include <QMouseEvent>
#include <QKeyEvent>
#include <QWheelEvent>
#include <QtWidgets>
#include <QColorDialog>
#include <iostream>
#include <cmath>

#include "editorwindow.h"

GameEngine::GameEngine(QWidget *parent) : QOpenGLWidget(parent)
{
    //mGameEnginePointer = this;
}

GameEngine::~GameEngine()
{
    mRunning = false;
    mTimer.stop();
    cleanup();
}

void GameEngine::init()
{

    mRunning = true;

    initializeOpenGLFunctions();

    //Must set this to get MouseMoveEvents without pressing a mouse key
    //this->setMouseTracking(true);

    // Enable depth buffer - must be done in GameEngine
    glEnable(GL_DEPTH_TEST);

    // Enable back face culling - must be done in GameEngine
    glEnable(GL_CULL_FACE);

    //Set up standard materials
    initMaterials();

    //Make and place camera
    mEditorCamera = new Camera();
    mEditorCamera->mTransform->setPosition(-3.0f, -2.0f, -20.0f);
    mEditorCamera->setBackgroundColor(0.4f, 0.4f, 0.4f, 1.0f);
    mEditorCamera->setMaterial(mMaterials[0]);
    currentActiveCamera = mEditorCamera;

    CreateGameCamera(mEditorCamera->mTransform->getPosition());
    mMainCamera->setBackgroundColor(0.4f, 0.4f, 0.4f, 1.0f);
    mMainCamera->setMaterial(mMaterials[0]);
    //Get the clear color from camera - the same as background color
    glClearColor(mEditorCamera->mBackgroundColor.x(),
                 mEditorCamera->mBackgroundColor.y(),
                 mEditorCamera->mBackgroundColor.z(),
                 mEditorCamera->mBackgroundColor.w());



    //Quick hack to make lots of cubes
    //The square root trick is because I want same number of rows and columns
    //    int noOfObjects{9};
    //    double squareRoot = sqrt(noOfObjects);
    GameObject *tempGeometry;
    tempGeometry = new OktaederBall(2);
    BasicSetOperations(tempGeometry, LookForDuplicatedName("Player_ball"), 0);
    tempGeometry->mTransform->setPosition(0.f, 5.f, 0.f);
    mGeometry.push_back(tempGeometry);

    playerBall = tempGeometry;
    SimpleScripting *playerScript;
    playerScript = new SimpleScripting(QString("../ORF_2017/Assets/testscript.js"), playerBall);
    playerBall->SetScriptComponent(playerScript);
    qDebug() << tempGeometry->getName() <<" was added.";

    playerBall->mScriptComponent->moveBall();


    int maxRow = 20;

    for(int i = 0; i < maxRow; i++){
        for(int j = 0; j < maxRow; j++){
            GameObject *lodTest = new Objloader(j*3, 0, i*3, LookForDuplicatedName(QString("Suzanne")));
            lodTest->setMaterial(mMaterials[0]);
            mGeometry.push_back(lodTest);
            LODindexes.push_back(mGeometry.size()-1);
        }
    }


    //Axes - cross gizmo:
    axes = new AxesGizmo();
    axes->setMaterial(mMaterials[0]);
    axisOnOff();

    skybox = new SkyBox();
    skybox->setMaterial(mMaterials[0]);
    skybox->mTransform->setScale(150.0f, 150.0f, 150.0f);

    mGeometry.push_back(skybox);

    // Starts the Game Loop
    // Use QBasicTimer because its faster than QTimer
    mTimer.start(16, this);


    //send signal to Hierarcy in GUI
    initHierarchy(mGeometry);
}

void GameEngine::setCameraColor()
{
    QColor color = QColor(static_cast<int>(mEditorCamera->mBackgroundColor.x()*255),
                          static_cast<int>(mEditorCamera->mBackgroundColor.y()*255),
                          static_cast<int>(mEditorCamera->mBackgroundColor.z()*255),
                          static_cast<int>(mEditorCamera->mBackgroundColor.w()*255));

    QString title = QString("Select Background color");
    color = QColorDialog::getColor(color, this, title);

    if (color.isValid())
    {
        mEditorCamera->setBackgroundColor(color.redF(), color.greenF(), color.blueF(), color.alphaF());

        glClearColor(mEditorCamera->mBackgroundColor.x(),
                     mEditorCamera->mBackgroundColor.y(),
                     mEditorCamera->mBackgroundColor.z(),
                     mEditorCamera->mBackgroundColor.w());
    }
}

void GameEngine::initMaterials()
{
    // Makes 4 default materials
    // 0.UnlitColor  1.UnlitTexture 2.LitColors 3.LitTexture
    // Only UnlitColor and UnlitTexture is implemented for now

    for (int no = 0; no < 4; no++)
    {
        mMaterials[no] = new Material(no);
    }
}

void GameEngine::cleanup()
{
    //Delete what we made in init()
    //    qDebug() << "Cleanup!";
    //    qDebug() << "Deleting" << mGeometry.size() << "objects";

    for (unsigned long noOfObjects = 0; noOfObjects < mGeometry.size(); ++noOfObjects)
    {
        delete mGeometry.at(noOfObjects);

        //The mGeometry vector deletes itself when going out of scope
    }
    mGeometry.clear();
    //    qDebug() << "mGeometry is now " << mGeometry.size() << "objects";

    if (axes)
    {
        delete axes;
        axes = nullptr;
        //        qDebug() << "Axes deleted";
    }
    for (int no = 0; no < 4; no++)
    {
        delete mMaterials[no];
        mMaterials[no] = nullptr;
        //        qDebug() << "Material" << no << "deleted";
    }

    if(mEditorCamera)
    {
        delete mEditorCamera;
        mEditorCamera = nullptr;
        //        qDebug() << "Camera deleted";
    }
}

GameObject *GameEngine::GetGameObjectFromList(int i)
{
    return mGeometry[i];
}


void GameEngine::AddObjectToScene(int index)
{

    //Should be made a different way.

    GameObject *tempGeometry;

    qDebug() << "Added gameobject with index: " << index;
    if (index == 1){
        tempGeometry = new Cube(4.0f, 3.0f, -4.0f, LookForDuplicatedName(objectName));
        tempGeometry->setMaterial(mMaterials[1]);
        mGeometry.push_back(tempGeometry);
        qDebug() << tempGeometry->getName() <<" was added.";
    }
    else if (index == 2){
        tempGeometry = new OktaederBall(2);
        BasicSetOperations(tempGeometry, LookForDuplicatedName(objectName), 0);
        mGeometry.push_back(tempGeometry);
        qDebug() << tempGeometry->getName() <<" was added.";

    }
    else if (index == 3){
        tempGeometry = new Plane(-25.0f, -2.0f, -25.0f, 50.0f, 50.0f, LookForDuplicatedName(objectName));
        tempGeometry->setMaterial(mMaterials[1]);
        tempGeometry->mTransform->setScale(1.0f, 0.0f, 1.0f);
        mGeometry.push_back(tempGeometry);
        qDebug() << tempGeometry->getName() <<" was added.";

    }
    //    else if (index == 4){
    //        tempGeometry = new Objloader(0.0f, -50.0f, 0.0f, LookForDuplicatedName(objectName));
    //        tempGeometry->setMaterial(mMaterials[0]);
    //        tempGeometry->mTransform->setScale(1.0f, 1.0f, 1.0f);
    //        mGeometry.push_back(tempGeometry);
    //        qDebug() << tempGeometry->getName() <<" was added.";

    //    }
    tempGameObject = tempGeometry;

    for (unsigned int i = 0; i < mGeometry.size(); i++){
        qDebug() << mGeometry[i]->getName() << "is gameobject number: " << i;
    }
}

void GameEngine::EditFilePath(char path)
{
    filepath = path;
    qDebug() << filepath << ": is set!";
}

void GameEngine::SetNewObjectName(QString name)
{
    objectName = name;
}

QString GameEngine::LookForDuplicatedName(QString tempName)
{
    bool isChecking = true;
    int additionalNumber = 0;
    QString temp = tempName;
    if (mGeometry.size() > 0){
        while(isChecking){
            for (unsigned int i = 0; i < mGeometry.size(); i++){
                if ((temp + QString::number(additionalNumber)) == mGeometry[i]->getName() || temp == mGeometry[i]->getName()){
                    additionalNumber++;
                } else {
                    isChecking = false;
                }
            }
        }
    }
    if(additionalNumber > 0)
        tempName = temp + QString::number(additionalNumber);
    return tempName;
}

GameObject* GameEngine::GetNewGameObject()
{
    return tempGameObject;
}

void GameEngine::ChangeCamera(bool inGameMode)
{
    if(inGameMode)
        currentActiveCamera = mMainCamera;
    else
        currentActiveCamera = mEditorCamera;

    paintGL();
}

std::vector<GameObject *> GameEngine::GetGeometryList()
{
    return mGeometry;
}

void GameEngine::SetSelectedObject(GameObject *selected)
{
    if(selected != currentSelected)
        currentSelected = selected;
}


void GameEngine::SpawnBoundingBoxOnSelected()
{
    if(currentBoundingBox != nullptr){
        delete currentBoundingBox;
        currentBoundingBox = nullptr;
    }

    if(currentSelected != nullptr){
        GameObject *tempGeometry = new BoundingBox(0.0f, 0.0f, 0.0f, "currentSelected->getName()");
        tempGeometry->setMaterial(mMaterials[0]);
        currentBoundingBox = tempGeometry;
        qDebug() << currentSelected->getName() <<" got a boundingBox.";
        mGeometry.push_back(tempGeometry);
        BoundingBoxTransform();
    }

}




//This function is called from the Qt framework
void GameEngine::initializeGL()
{
    initializeOpenGLFunctions();
    init();
}

void GameEngine::resizeGL(int w, int h)
{
    mEditorCamera->setAspectRatio(w, h);
    mMainCamera->setAspectRatio(w, h);
}

void GameEngine::paintGL()
{

    //paintGL is the Qt function called from the internal Qt loop

    // Clear color and depth buffer
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glClearColor(currentActiveCamera->mBackgroundColor.x(),
                 currentActiveCamera->mBackgroundColor.y(),
                 currentActiveCamera->mBackgroundColor.z(),
                 currentActiveCamera->mBackgroundColor.w());

    // Calculate view transformation
    if (!currentActiveCamera)
        qDebug() << "Rendering without camera!";

    // cameras viewMatrix - same for all objects
    viewMatrix = currentActiveCamera->getMatrix();
    // cameras projectionMatrix - same for all objects
    projectionMatrix = currentActiveCamera->getPerspectiveMatrix();

    //set position of skybox to that of camera
    //Not 100% correct yet.
    Vec3 temp = currentActiveCamera->mTransform->getPosition();
    temp.z = -temp.z;
    temp.x = -temp.x;
    skybox->mTransform->setPosition(temp);

    //Draw all geometry in mGeometry vector:
    GameObject *tempGeometry;



    foreach (auto &tempGameObject, mGeometry) {
        tempGeometry = tempGameObject;

        bool testingFrustum = true;

        if(testingFrustum){
            if(tempGeometry->GetType() != QString("Camera")){
                Vec3 frustumNormals[6];

                for(int i = 0; i < 6; i++){
                    frustumNormals[i] = mMainCamera->GetFrustumNormals(i);
                }

                Vec3 pos = tempGeometry->mTransform->getPosition();
                Vec3 posToCam = (mMainCamera->mTransform->getPosition() - pos).normalized();
                Vec3 posToCamFar = (mMainCamera->mTransform->getPosition() + Vec3(0,0,-1) * mMainCamera->GetFarPlane() - pos).normalized();
                Vec3 posToCamNear = (mMainCamera->mTransform->getPosition() + Vec3(0,0,-1) * mMainCamera->GetNearPlane() - pos).normalized();

                float nearTest = (frustumNormals[0]).normalized() * posToCamNear;
                float farTest =  (frustumNormals[1]).normalized() * posToCamFar;
                float rightTest =(frustumNormals[2]).normalized() * posToCam;
                float leftTest = (frustumNormals[3]).normalized() * posToCam;
                float topTest =  (frustumNormals[4]).normalized() * posToCam;
                float bottomTest=(frustumNormals[5]).normalized() * posToCam;


                bool nearBool = nearTest <= 0 ? true : false;
                bool farBool = farTest <= 0 ? true : false;
                bool rightBool = rightTest <= 0 ? true : false;
                bool leftBool = leftTest <= 0 ? true : false;
                bool topBool = topTest <= 0 ? true : false;
                bool bottomBool = bottomTest <= 0 ? true : false;


                if(nearBool && farBool && rightBool && leftBool && topBool && bottomBool){
                    qDebug() << "has beem drawm";
                    tempGeometry->setViewMatrix(viewMatrix);
                    tempGeometry->setprojectionMatrix(projectionMatrix);
                    tempGeometry->drawGeometry();
                }
            }
            else{
                tempGeometry->setViewMatrix(viewMatrix);
                tempGeometry->setprojectionMatrix(projectionMatrix);
                tempGeometry->drawGeometry();
            }
        }
        else{
            tempGeometry->setViewMatrix(viewMatrix);
            tempGeometry->setprojectionMatrix(projectionMatrix);
            tempGeometry->drawGeometry();
        }



        //if Axis editor button is on:
        if (axisOn)
        {
            axes->setViewMatrix(viewMatrix);
            axes->setprojectionMatrix(projectionMatrix);
            axes->drawGeometry();
        }
    }
}

void GameEngine::BasicSetOperations(GameObject *tempGeometry, QString name, int materialType)
{
    tempGeometry->setMaterial(mMaterials[materialType]);
    tempGeometry->setName(name);
}

void GameEngine::BoundingBoxTransform()
{
    Vec3 pos = currentSelected->mTransform->getPosition();
    Vec3 scale = currentSelected->mTransform->getScale();
    Vec3 rot = currentSelected->mTransform->getRotation();
    currentBoundingBox->mTransform->setPosition(pos.x, pos.y, pos.z);
    currentBoundingBox->mTransform->setScale(scale.x, scale.y, scale.z);
    currentBoundingBox->mTransform->setRotation(rot.x, rot.y, rot.z);
}

void GameEngine::LoadScene(QString file)
{

    tempSceneManager->LoadScene(mGeometry, file, GetMaterial(0));
    qDebug() << "-------------------------";
    qDebug() << "Current gameObjects in the list: ";
    qDebug() << "-------------------------";
    for (uint i = 0; i < mGeometry.size(); i++){
        qDebug() << QString(mGeometry[i]->getName());
    }
}

void GameEngine::SaveScene(QString file)
{
    qDebug() << "-------------------------";
    qDebug() << "Saving these gameObjects: ";
    qDebug() << "-------------------------";
    for (uint i = 0; i < mGeometry.size(); i++){
        qDebug() << QString(mGeometry[i]->getName());
    }
    tempSceneManager->SaveCurrentScene(mGeometry, file);
}

Material *GameEngine::GetMaterial(int index)
{
    if (index > 4 || index < 0) index = 0;

    return mMaterials[index];
}

void GameEngine::CreateGameCamera(Vec3 startPosition)
{
    Camera *tempCamera;
    tempCamera = new Camera(800, 1200,  1.0f,  10.0f, 45.0f);
    BasicSetOperations(tempCamera, LookForDuplicatedName("MainCamera"), 0);
    mMainCamera = tempCamera;
    mMainCamera->mTransform->setPosition(Vec3(0,0,0));
    qDebug() << tempCamera->getName() <<" was added.";
    mGeometry.push_back(mMainCamera);

    GameObject *tempGeometry;
    tempGeometry = new Frustum(mMainCamera);
    tempGeometry->setMaterial(mMaterials[0]);
    frustumInScene = tempGeometry;
    mGeometry.push_back(frustumInScene);

}

void GameEngine::axisOnOff()
{
    // qDebug() << "Axis on/off";
    axisOn = !axisOn;
}

void GameEngine::orthographicOnOff()
{
    ortho = !ortho;
    if (ortho){
        mEditorCamera->setAspectRatio(1280, 720, true);
        mMainCamera->setAspectRatio(1280, 720,  true);
    }
    else{
        mEditorCamera->setAspectRatio(1280, 720);
        mMainCamera->setAspectRatio(1280, 720);
    }

}

void GameEngine::wireFrameOnOff()
{
    //qDebug() << "Wireframe on/off";
    wireFrame = !wireFrame;
    GameObject *tempGeometry;
    foreach (auto &tempGameObject, mGeometry) {
        tempGeometry = tempGameObject;
        tempGeometry->setRendermode(wireFrame);
    }
}

void GameEngine::timerEvent(QTimerEvent *)
{

    int vertAmount = 0;
    for(uint i = 0; i < LODindexes.size(); i++){
        mGeometry[LODindexes[i]]->SetLODstate(mMainCamera->mTransform->getPosition());
        vertAmount += mGeometry[LODindexes[i]]->GetVertAmount();
    }
    QString text = QString::number(vertAmount) + " : Vertex count | ";
    qDebug() << text;

    if(currentBoundingBox != nullptr)
        BoundingBoxTransform();


    playerBall->mScriptComponent->moveBall();

    frustumInScene->mTransform->setPosition(mMainCamera->mTransform->getPosition());

    handleKeys();
    // Request an update for the QOpenGL widget

    update();
}

void GameEngine::mouseMoveEvent(QMouseEvent *event)
{
    //using mouseXYlast as deltaXY so we don't need extra variables
    mouseXlast = event->pos().x() - mouseXlast;
    mouseYlast = event->pos().y() - mouseYlast;

    //if delta is to big the movement will be jerky
    //Happens if mouse is moved much between presses.
    if (mouseXlast > 40 || mouseYlast > 40 || mouseXlast < -40 || mouseYlast < -40)
    {
        mouseXlast = 0;
        mouseYlast = 0;
    }

    //qDebug() << "dX: "<< mouseXlast << ", dY: "<< mouseYlast;
    mEditorCamera->mTransform->rotate(0.0f,cameraSpeed*mouseXlast,0.0f);
    mEditorCamera->mTransform->rotate(cameraSpeed*mouseYlast,0.0f,0.0f);

    mouseXlast = event->pos().x();
    mouseYlast = event->pos().y();
}

void GameEngine::keyPressEvent(QKeyEvent *event)
{
    //move camera
    if(event->key() == Qt::Key_A)
    {
        mLeft = true;
    }
    if(event->key() == Qt::Key_D)
    {
        mRight  = true;
    }
    if(event->key() == Qt::Key_W)
    {
        mUp = true;
    }
    if(event->key() == Qt::Key_S)
    {
        mDown = true;
    }
}

void GameEngine::keyReleaseEvent(QKeyEvent *event)
{
    //move camera
    if(event->key() == Qt::Key_A)
    {
        mLeft = false;
    }
    if(event->key() == Qt::Key_D)
    {
        mRight  = false;
    }
    if(event->key() == Qt::Key_W)
    {
        mUp = false;
    }
    if(event->key() == Qt::Key_S)
    {
        mDown = false;
    }
}

void GameEngine::wheelEvent(QWheelEvent* event)
{
    //http://doc.qt.io/qt-4.8/qwheelevent.html
    int numDegrees = event->delta() / 8;
    float numSteps = numDegrees / 15;

    if (event->orientation() == Qt::Horizontal) {
        mEditorCamera->mTransform->translate(numSteps, 0.0, 0.0);
    } else {
        mEditorCamera->mTransform->translate(0.0, 0.0, numSteps);
    }
    event->accept();
}

void GameEngine::handleKeys()
{
    //move camera
    if(mLeft)
    {
        mEditorCamera->mTransform->translate(0.2f, 0.0, 0.0);
    }
    if(mRight)
    {
        mEditorCamera->mTransform->translate(-0.2f, 0.0, 0.0);
    }
    if(mUp)
    {
        mEditorCamera->mTransform->translate(0.0, 0.0, 0.2f);
    }
    if(mDown)
    {
        mEditorCamera->mTransform->translate(0.0, 0.0, -0.2f);
    }
}
