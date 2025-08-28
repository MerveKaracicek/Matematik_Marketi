document.addEventListener("DOMContentLoaded", function() {

    // ANA MENÜ --> OYUN
    const startBtn = document.querySelector(".startBtn");
    if(startBtn){
        startBtn.addEventListener("click", async function() {
            const response = await fetch('/Game/StartGame', { method: 'POST' });
            if (!response.ok) {
                console.error('Fetch hatası:', response.status);
                return;
            }
            const data = await response.json();
            window.location.href = '/Game/Play?gameId=' + data.gameId;
        });
    }

    // OYUN SAYFASI --> ALIŞVERIŞ LISTESI
    const listBtn = document.querySelector(".listBtn");
    if(listBtn){
        listBtn.addEventListener("click", function() {
            const gameId = this.dataset.gameid;
            window.location.href = '/Home/AlisverisListesi?gameId=' + gameId;
        });
    }

    // Tüm TAMAM butonları
    document.querySelectorAll(".btn").forEach(function(btn){
        const type = btn.dataset.btnType; // her butona ekle
        btn.addEventListener("click", function(){
            const gameId = this.dataset.gameid;

            if(type === "soru-tamam" || type === "alisveris-tamam"){
                window.location.href = '/Game/Play?gameId=' + gameId;
            }
            // Diğer türler eklenebilir
        });
    });

    // OYUN SAYFASI --> SORU EKRANI (ürünler)
 document.querySelectorAll(".product").forEach(function(btn) {
    const productId = btn.dataset.productid;
    if (!productId) return; // Boşsa tıklama ekleme

    btn.addEventListener("click", function() {
        const gameId = this.dataset.gameid;
        window.location.href = '/Game/Soru?gameId=' + gameId + '&productId=' + productId;
    });
});

    // BİTİŞ EKRANI --> ANA MENÜ
    const cikisBtn = document.querySelector(".cikisBtn");
    if(cikisBtn){
        cikisBtn.addEventListener("click", function(){
            window.location.href= '/Home/AnaMenu';
        });
    }

});
