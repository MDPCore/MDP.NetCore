// MDP
var mdp = mdp || {};

// MDP.Liff
mdp.liff = {

    // inti
    init: function (liffId, returnUrl) {

        // liff
        return liff.init({

            // liffId
            liffId: liffId
        })
        .then(() => {

            // require
            if (liff.isInClient() == false) { window.location = returnUrl; return; }
            if (liff.isLoggedIn() == false) { window.location = returnUrl; return; }

            // signin
            var signinURL = new URL("/signin-liff", window.location.href);
            signinURL.searchParams.append("returnUrl", returnUrl);
            signinURL.searchParams.append("access_token", liff.getAccessToken());
            signinURL.searchParams.append("id_token", liff.getIDToken());
            window.location = signinURL.href;
        });
    }
}