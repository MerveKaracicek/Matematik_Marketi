document.addEventListener("DOMContentLoaded", function() {

    // ANA MENÜ --> OYUN
    const startBtn = document.querySelector(".startBtn");
    if (startBtn) {
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

    // OYUN SAYFASI --> ALIŞVERİŞ LİSTESİ
    const listBtn = document.querySelector(".listBtn");
    if (listBtn) {
        listBtn.addEventListener("click", function() {
            const gameId = this.dataset.gameid;
            window.location.href = '/Home/AlisverisListesi?gameId=' + gameId;
        });
    }

    // Tüm TAMAM butonları
    document.querySelectorAll(".btn").forEach(function(btn) {
        const type = btn.dataset.btnType;
        btn.addEventListener("click", function() {
            const gameId = this.dataset.gameid;

            if (type === "soru-tamam" || type === "alisveris-tamam") {
                window.location.href = '/Game/Play?gameId=' + gameId;
            }
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

    // OYUN SAYFASI --> CEVAP GÖNDERME
    const submitBtn = document.querySelector(".submitAnswer");
    const answerInput = document.querySelector(".answer-input");

    if (submitBtn && answerInput) {
        submitBtn.addEventListener("click", async function() {
            const gameId = this.dataset.gameid;
            const productId = this.dataset.productid;
            const userAnswer = answerInput.value.trim();

            if (!userAnswer) return;
            
            const resultEl=document.querySelector(".result");
            resultEl.style.display="block";

            try {
                const response = await fetch('/Game/CevapKontrol', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        GameId: gameId,
                        ProductId: productId,
                        UserAnswer: userAnswer
                    })
                });

                const data = await response.json();
                console.log("Backend cevabı:", data);
                console.log("data:", data);
                console.log("data.correct:", data.correct);
                console.log("typeof data.correct:", typeof data.correct);

                
                if(data.correct==true){

                    resultEl.style.color="green";
                    resultEl.textContent="Cevap Doğru!";
                    }
                    else{
                        resultEl.style.color="red";
                        resultEl.textContent="Cevap Yanlış, Tekrar Deneyin!";


                    }

                    answerInput.value = "";
                        
    

        }  catch (err) {
                console.error("Fetch hatası:", err);
            }

        });
    }

    // BİTİŞ EKRANI --> ANA MENÜ
    const cikisBtn = document.querySelector(".cikisBtn");
    if (cikisBtn) {
        cikisBtn.addEventListener("click", function() {
            window.location.href = '/Home/AnaMenu';
        });
    }

});
