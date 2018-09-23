#ifndef GAMEENGINE_H
#define GAMEENGINE_H

#include <QOpenGLWidget>
#include <QOpenGLFunctions>
#include <QMatrix4x4>
#include <QBasicTimer>

#include <QOpenGLShaderProgram>
#include <QOpenGLTexture>

#include <vector>

#include "camera.h"
#include "axesgizmo.h"
#include "skybox.h"
#include "cube.h"
#include "plane.h"
#include "circlesphere.h"
#include "shaderprogram.h"
#include "objloader.h"
#include "oktaederball.h"
#include "boundingbox.h"
#include "simplegameobject.h"
#include "scenemanager.h"
#include "frustum.h"
#include "simplescripting.h"



//This is the Game-Engine, where everything is put together.
//This is a PROTOTYPE version, and is in need of cleaning and new systems.

class GameEngine : public QOpenGLWidget, protected QOpenGLFunctions
{
    Q_OBJECT

public:
    explicit GameEngine(QWidget *parent = 0);
    ~GameEngine();

    void init();
    void initMaterials();
    //void initTextures();
    void cleanup();

    bool running() { return mRunning; }
    void quit() { mRunning = false; }

    GameObject* GetGameObjectFromList(int i);

    void AddObjectToScene(int index);
    void EditFilePath(char path);
    void SetNewObjectName(QString name);

    QString LookForDuplicatedName(QString tempName);

    GameObject *GetNewGameObject();

    void ChangeCamera(bool inGameMode);
    std::vector<GameObject*> GetGeometryList();

    void SetSelectedObject(GameObject *selected);
    void SpawnBoundingBoxOnSelected();
    void BoundingBoxTransform();

    void LoadScene(QString file);
    void SaveScene(QString file);


    Material *GetMaterial(int index);


    GameObject* GetPlayerBall() { return playerBall;}



signals:
    void initHierarchy(std::vector<GameObject*> &mGeometry);

public slots:
    void axisOnOff();
    void wireFrameOnOff();
    void setCameraColor();
    void orthographicOnOff();

protected:
    //This is the "handleEvents" part:
    void mouseMoveEvent(QMouseEvent *event) Q_DECL_OVERRIDE;
    void keyPressEvent(QKeyEvent *event) Q_DECL_OVERRIDE;
    void keyReleaseEvent(QKeyEvent *event) Q_DECL_OVERRIDE;
    void wheelEvent(QWheelEvent *event) Q_DECL_OVERRIDE;

    void handleKeys();

    void timerEvent(QTimerEvent *e) Q_DECL_OVERRIDE;

    void initializeGL() Q_DECL_OVERRIDE;
    void resizeGL(int w, int h) Q_DECL_OVERRIDE;
    void paintGL() Q_DECL_OVERRIDE;

    void BasicSetOperations(GameObject *tempGeometry, QString name, int materialType);


private:
    QBasicTimer mTimer;

    Material *mMaterials[4];

    Camera *mEditorCamera;

    SceneManager *tempSceneManager = new SceneManager;

    QMatrix4x4 viewMatrix;
    QMatrix4x4 projectionMatrix;

    std::vector<GameObject*> mGeometry;

    GameObject *axes;
    GameObject *skybox;

    QString* fragmentShaderFileName;
    QString* vertexShaderFileName;

    bool mRunning;

    bool axisOn = true;
    bool wireFrame = false;
    bool ortho = false;

    int mouseXlast{};
    int mouseYlast{};
    float cameraSpeed = 0.2f;

    //This really! should be made anoter way!
    //Quick hack ahead:
    bool mW = false;
    bool mA = false;
    bool mS = false;
    bool mD = false;
    bool mUp = false;
    bool mDown = false;
    bool mLeft = false;
    bool mRight = false;
    bool mQ = false;
    bool mE = false;

    char filepath;
    QString objectName = QString("");

    GameObject *tempGameObject = nullptr;
    Camera *mMainCamera = nullptr;

    Camera *currentActiveCamera = nullptr;

    GameObject *frustumInScene = nullptr;

    void CreateGameCamera(Vec3 startPosition);

    //Used to spawn boundingBox
    GameObject *currentSelected = nullptr;
    GameObject *currentBoundingBox = nullptr;


    GameObject *playerBall = nullptr;
    std::vector<int> LODindexes;



};

#endif // GAMEENGINE_H
