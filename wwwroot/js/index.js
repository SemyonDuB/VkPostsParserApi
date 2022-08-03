const appId = "8208494";
const uri = `https://oauth.vk.com/authorize?client_id=${appId}&display=page&redirect_uri=${window.location.origin}&scope=wall&response_type=code&v=5.131`;

window.onload = () => {
    let params = new URLSearchParams(window.location.search);
    let code = params.get("code");
    if (code == null)
        return;

    fetch(`VkPosts/GetNumberOccurrencesLetters/?code=${code}`)
        .then(data => data.json())
        .then(response => {
            document.getElementById("result").innerHTML = JSON.stringify(response, null, 4)
                .split("\n")
                .join("<br/>");
        });
}