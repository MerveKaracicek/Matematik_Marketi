//ANA MENÜ -- > OYUN SAYFASI
document.addEventListener("DOMContentLoaded", function() {
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
});




//OYUN SAYFASI --> ALIŞVERİŞ LİSTESİ
document.addEventListener("DOMContentLoaded", function() {
    const listBtn = document.querySelector(".listBtn");
    if(listBtn){
        listBtn.addEventListener("click", function() {
            const gameId = this.dataset.gameid;
            window.location.href = '/Home/AlisverisListesi?gameId=' + gameId;
        });
    }
});


//ALIŞVERİŞ LİSTESİ --> OYUN SAYFASI
//SORU EKRANI --> OYUN SAYFASI
//BİTİS SAYFASI --> OYUN SAYFASI

document.querySelectorAll(".btn").forEach(function(btn){
    btn.addEventListener("click", function(){
        const gameId = this.dataset.gameid;
        if(gameId){
            window.location.href = '/Game/Play?gameId=' + gameId;
        }
    });
});


//OYUN SAYFASI --> SORU EKRANI 
document.querySelectorAll(".product").forEach(function(btn){
    btn.addEventListener("click", function(){
        const gameId = this.dataset.gameid; // gameId eklenmeli
        window.location.href = '/Home/Soru?gameId=' + gameId;
    });
});


//BİTİŞ EKRANI --> ANA MENÜ

document.addEventListener("DOMContentLoaded",function(){
    const cikisBtn=document.querySelector(".cikisBtn");
    if(cikisBtn){
        cikisBtn.addEventListener("click",function(){
            window.location.href= '/Home/AnaMenu';
        });
    }
});