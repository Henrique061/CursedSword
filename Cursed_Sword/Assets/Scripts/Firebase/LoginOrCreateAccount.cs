using System.Text.RegularExpressions;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginOrCreateAccount : MonoBehaviour
{
    [SerializeField] TMP_InputField userEmail;
    [SerializeField] TMP_InputField userPass;
    [SerializeField] TextMeshProUGUI returnText;
    [SerializeField] MainMenuLogin mainMenu;

    private int triesCheck = 0;

    void Start()
    {
        // se ja existe um usuario logado, ativa o painel para desconectar
        if (AuthManager.instance.CheckIfThePlayerIsLoggedIn() == true)
        {
            string playerName = AuthManager.instance.GetPlayerName() == "" ?
                    AuthManager.instance.GetPlayerEmail() :
                    AuthManager.instance.GetPlayerName() + ".";
            mainMenu.EnableDisconnectPanel("Connected as " + playerName);
        }
    }

    public void CreateAccount()
    {
        bool emailIsOK = ValidateEmail(userEmail.text);
        bool passwordIsOK = ValidatePassword(userPass.text);
        if (emailIsOK && passwordIsOK)
        {
            // cria a conta do usuário            
            AuthManager.instance.CreateNewUser(userEmail.text, userPass.text);
            // verifica, a cada 0.3 segundos, se já tem resultado
            triesCheck = 0;
            InvokeRepeating("CheckIfCreateResultIsOk", 0.3f, 0.3f);
        }
    }

    public void LoginWithEmailAndPassword()
    {
        AuthManager.instance.LoginWithEmail(userEmail.text, userPass.text);
        InvokeRepeating("CheckIfCreateResultIsOk", 0.3f, 0.3f);
    }

    public void DisconnectUser()
    {
        AuthManager.instance.Disconnect();
        InvokeRepeating("CheckIfCreateResultIsOk", 0.3f, 0.3f);
    }

    public void CheckIfCreateResultIsOk()
    {
        if (AuthManager.instance.GetFinalResult() != "")
        {
            CancelInvoke();
            returnText.text = AuthManager.instance.GetFinalResult();
            StatusConnection sc = AuthManager.instance.GetStatusConnection();
            if (sc == StatusConnection.connected)
            {
                string playerName = AuthManager.instance.GetPlayerName() == "" ?
                    AuthManager.instance.GetPlayerEmail() :
                    AuthManager.instance.GetPlayerName() + ".";
                mainMenu.EnableDisconnectPanel("Connected as " + playerName);

            }
            else if (sc == StatusConnection.logedOut)
            {
                mainMenu.DisableDisconnectPanel();
            }
        }
        // tenta 20 vezes. se falhar, para
        if (triesCheck == 20)
        {
            CancelInvoke();
            returnText.text = "Falha na tentativa de login. Tente novamente mais tarde.";
        }
    }



    // code from Anurag on
    // https://stackoverflow.com/questions/34715501/validating-password-using-regex-c-sharp
    // validate password with at least 8 
    private bool ValidatePassword(string password)
    {
        var input = password;
        // testa se o password esta vazio
        if (string.IsNullOrWhiteSpace(input))
        {
            returnText.text = "Password should not be empty";
            return false;
        }
        // expressoes regex com os caracteres a serem testados
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMiniMaxChars = new Regex(@".{8,15}");
        var hasLowerChar = new Regex(@"[a-z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        // testa se possui letra minuscula
        if (!hasLowerChar.IsMatch(input))
        {
            returnText.text = "Password should contain At least one lower case letter";
            return false;
        }
        // testa se possui letra maiuscula
        else if (!hasUpperChar.IsMatch(input))
        {
            returnText.text = "Password should contain At least one upper case letter";
            return false;
        }
        // testa se esta entre 8 e 15 caracteres
        else if (!hasMiniMaxChars.IsMatch(input))
        {
            returnText.text = "Password should not be less than 8 or greater than 15 characters";
            return false;
        }
        // testa se possui numero
        else if (!hasNumber.IsMatch(input))
        {
            returnText.text = "Password should contain At least one numeric value";
            return false;
        }
        // testa se possui um caractere especial
        else if (!hasSymbols.IsMatch(input))
        {
            returnText.text = "Password should contain At least one special case characters";
            return false;
        }
        // se estiver tudo certo, retorna verdadeiro
        else
        {
            return true;
        }
    }
    // testa apenas se possui o arroba
    public bool ValidateEmail(string email)
    {
        if (email.IndexOf('@') <= 0)
        {
            returnText.text = "Incorrect email formatting!";
            userEmail.Select();
            return false;
        }
        return true;
    }

    public void LoginWithGoogle()
    {
        triesCheck = 0;
        returnText.text = "Trying to Authenticate...";
        AuthManager.instance.AuthenticateWithGoogle();
        InvokeRepeating(nameof(CheckIfAuthenticateWithGoogleIsOk), 0.5f, 0.5f);
    }

    public void CheckIfAuthenticateWithGoogleIsOk()
    {
        if (triesCheck >= 200)
        {
            returnText.text = "Unable to LogIn. Try again later!";
            CancelInvoke();
            return;
        }
        string[] user = AuthManager.instance.GetGoogleUserData();
        if (user == null)
        {
            return;
        }
        else
        {
            CancelInvoke();
            returnText.text = "UserID:" + user[0] + "\n" + "Display Name: " + user[1];
            string playerName = AuthManager.instance.GetPlayerName() == "" ?
                    AuthManager.instance.GetPlayerEmail() :
                    AuthManager.instance.GetPlayerName() + ".";
            mainMenu.EnableDisconnectPanel("Connected as " + playerName);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("MainGame");
    }
}
