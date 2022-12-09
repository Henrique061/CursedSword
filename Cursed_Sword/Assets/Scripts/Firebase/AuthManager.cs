using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;

public enum StatusConnection { connected, cancell, fail, error, logedOut }
public class AuthManager : MonoBehaviour
{
    // aplicando singleton no metodo
    public static AuthManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    FirebaseAuth auth;
    FirebaseUser user { get; set; }
    string finalResult = "";
    StatusConnection statusConnection;
    void Start()
    {
        statusConnection = new StatusConnection();
        auth = FirebaseAuth.DefaultInstance;
    }

    public void CreateNewUser(string email, string passoword)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, passoword).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                finalResult = "A criação do usuário foi cancelada";
                statusConnection = StatusConnection.cancell;
                return;
            }
            if (task.IsFaulted)
            {
                finalResult = "Erro na criação de usuário" + task.Exception.Message.ToString();
                statusConnection = StatusConnection.error;
                return;
            }
            user = task.Result;
            finalResult = string.Format("Usuário criado com sucesso: {0} ({1})", user.DisplayName, user.UserId);
            statusConnection = StatusConnection.connected;
        });
    }

    public string GetFinalResult()
    {
        return finalResult;
    }

    public StatusConnection GetStatusConnection()
    {
        return statusConnection;
    }

    public void LoginWithEmail(string email, string password)
    {
        if (auth != null)
        {
            auth.SignOut();
            statusConnection = StatusConnection.logedOut;
        }
        finalResult = "";
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                finalResult = "Operação cancelada!";
                statusConnection = StatusConnection.cancell;
                return;
            }
            if (task.IsFaulted)
            {
                finalResult = "Erro ao fazer o login: " + task.Exception.Message.ToString();
                statusConnection = StatusConnection.error;
                return;
            }
            user = task.Result;
            finalResult = string.Format("Usuário {0} autenticado com sucesso", user.DisplayName);
            statusConnection = StatusConnection.connected;
        });
    }

    public void Disconnect()
    {
        finalResult = "";
        if (auth != null)
        {
            auth.SignOut();
            statusConnection = StatusConnection.logedOut;
            finalResult = "Usuário desconectado com sucesso!";
        }
    }

    public void AuthenticateWithGoogle()
    {
        if (auth != null)
        {
            auth.SignOut();
            statusConnection = StatusConnection.logedOut;
        }
        // usado para controlar o retorno
        finalResult = "";
        // configuracao do google solicitando o token e passando o webclientid
        /*GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            // esse valor de baixo esta no arquivo google-services.json
            // na parte oauth_client identificado como client_id (junto do client_type 3)
            WebClientId = "690657509872-3gt52jnt961qv1pn3h9iaq58r6p4frc0.apps.googleusercontent.com"
        };*/
        // variavel do tipo Task<GoogleSignInUser> que e responsavel
        // pelo login com as configuracoes acima
        //var signIn = GoogleSignIn.DefaultInstance.SignIn();
        // variavel que recebe o feedback se deu algum erro ou se deu certo
        var signInCompleted = new TaskCompletionSource<FirebaseUser>();

        /*signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
            }
            else
            {
                // dando certo, requisita o GoogleIdToken e armazena na credential
                Credential credential = GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                // utiliza o objeto Firebase.Auth para logar com a credencial fornecida
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask => {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                        finalResult = "Sucesso"; // registra o sucesso
                        statusConnection = StatusConnection.connected;
                    }
                });
            }
        });*/
    }

    // retorna os dados do usuario quando deu certo o login
    public string[] GetGoogleUserData()
    {
        if (finalResult != "")
        {
            string[] userData = new string[2];
            if (auth != null)
            {
                // userid e um identificador unico na google cloud
                userData[0] = auth.CurrentUser.UserId;
                // displayname e o nome que esta no google
                userData[1] = auth.CurrentUser.DisplayName;
                return userData;
            }
        }
        return null;
    }

    public string GetPlayerName()
    {
        return auth.CurrentUser.DisplayName;
    }

    public string GetPlayerId()
    {
        return auth.CurrentUser.UserId;
    }

    public string GetPlayerEmail()
    {
        return auth.CurrentUser.Email;
    }

    public bool CheckIfThePlayerIsLoggedIn()
    {
        if (auth.CurrentUser == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
