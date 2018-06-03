using Facebook.Unity;

public class HomeController : BaseController {
    private const int PLAY = 0;
    private const int FACEBOOK = 1;

    public void OnClick(int index)
    {
        switch (index)
        {
            case PLAY:
                CUtils.LoadScene(1);
                break;
            case FACEBOOK:
                if (FB.IsLoggedIn || !ConfigController.Config.enableFacebookFeatures)
                    CUtils.LikeFacebookPage(ConfigController.Config.facebookPageID);
                else
                    FacebookController.instance.LoginFacebook();
                break;
        }
        Sound.instance.PlayButton();
    }

}
