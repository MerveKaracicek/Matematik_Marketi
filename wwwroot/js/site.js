document.addEventListener("DOMContentLoaded", function () {
  // Oyun sayfasında backend’den gelen can ve heart görselleri
  const sidebar = document.querySelector(".sidebar");
  const can = sidebar ? parseInt(sidebar.dataset.lives) : 3; // backend’den gelen can, yoksa 3 default
  const hearts = document.querySelectorAll(".carts img");

  // İlk yüklemede görselleri can sayısına göre ayarla
  hearts.forEach((h, index) => {
    h.style.display = index < can ? "inline" : "none";
  });

  const expressionEl = document.querySelector(".expression");
  if (expressionEl) {
    expressionEl.textContent = expressionEl.textContent.replace(/\*/g, "x");
  }

  // ANA MENÜ --> OYUN
  const startBtn = document.querySelector(".startBtn");
  if (startBtn) {
    startBtn.addEventListener("click", async function () {
      const response = await fetch("/Game/StartGame", { method: "POST" });
      if (!response.ok) {
        console.error("Fetch hatası:", response.status);
        return;
      }
      const data = await response.json();
      window.location.href = "/Game/Play?gameId=" + data.gameId;
    });
  }

  // OYUN SAYFASI --> ALIŞVERİŞ LİSTESİ
  const listBtn = document.querySelector(".listBtn");
  if (listBtn) {
    listBtn.addEventListener("click", function () {
      const gameId = this.dataset.gameid;
      window.location.href = "/Home/AlisverisListesi?gameId=" + gameId;
    });
  }

  // Tüm TAMAM butonları
  document.querySelectorAll(".btn").forEach(function (btn) {
    const type = btn.dataset.btnType;
    btn.addEventListener("click", function () {
      const gameId = this.dataset.gameid;
      if (type === "soru-tamam" || type === "alisveris-tamam") {
        window.location.href = "/Game/Play?gameId=" + gameId;
      }
    });
  });

  // OYUN SAYFASI --> SORU EKRANI (ürünler)

  document.querySelectorAll(".product").forEach(function (btn) {
    const productId = btn.dataset.productid;
    if (!productId) return;

    btn.addEventListener("click", function () {
      const gameId = this.dataset.gameid;
      window.location.href =
        "/Game/Soru?gameId=" + gameId + "&productId=" + productId;
    });
  });

  // OYUN SAYFASI --> CEVAP GÖNDERME
  const submitBtn = document.querySelector(".submitAnswer");
  const answerInput = document.querySelector(".answer-input");

  if (submitBtn && answerInput) {
    submitBtn.addEventListener("click", async function () {
      const gameId = this.dataset.gameid;
      const productId = this.dataset.productid;
      const userAnswer = answerInput.value.trim();

      if (!userAnswer) return;

      const resultEl = document.querySelector(".result");
      resultEl.style.display = "block";

      try {
        const response = await fetch("/Game/CevapKontrol", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            GameId: gameId,
            ProductId: productId,
            UserAnswer: userAnswer,
          }),
        });

        const data = await response.json();

        // Cevap doğru/yanlış mesajı
        if (data.correct) {
          resultEl.style.color = "green";
          resultEl.textContent = "Cevap Doğru!";
        } else {
          resultEl.style.color = "red";
          resultEl.textContent = "Cevap Yanlış, Tekrar Deneyin!";
        }

        answerInput.value = "";

        // Oyun durumu kontrolü ve bitiş ekranına yönlendirme
        try {
          const statusResponse = await fetch("/Game/CheckGameStatus", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(gameId),
          });

          const statusData = await statusResponse.json();

          if (statusData.status === "Won" || statusData.status === "Lost") {
            window.location.href = "/Home/Bitis?gameId=" + gameId;
          }
        } catch (err) {
          console.error("Game status fetch hatası:", err);
        }
      } catch (err) {
        console.error("Fetch hatası:", err);
      }
    });
  }

  const newGameBtn = document.getElementById("newGameBtn");
  if (newGameBtn) {
    newGameBtn.addEventListener("click", async function () {
      const gameId = this.getAttribute("data-gameid");

      try {
        const response = await fetch("/Game/StartGame", { method: "POST" });
        if (!response.ok) {
          console.error("Yeni oyun başlatılamadı");
          return;
        }

        const newGame = await response.json();
        // Yeni oyunun ekranına yönlendir
        window.location.href = "/Game/Play?gameId=" + newGame.gameId;
      } catch (err) {
        console.error("Yeni oyun hatası:", err);
      }
    });
  }

  // Çıkış Butonu
  const exitBtn = document.getElementById("exitBtn");
  if (exitBtn) {
    exitBtn.addEventListener("click", function () {
      window.location.href = "/Home/AnaMenu"; // Ana menüye yönlendir
    });
  }
});
