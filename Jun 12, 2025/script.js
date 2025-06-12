const outputDiv = document.getElementById("output");

const stockData = {
  AAPL: 195.37,
  MSFT: 320.50,
  GOOGL: 2801.12
};

function simulateStockFetchWithCallback(symbol, callback) {
  setTimeout(() => {
    const price = stockData[symbol] || "Stock not found";
    callback(`${symbol} Price (Callback): $${price}`);
  }, 1000);
}

function getStockWithCallback() {
  simulateStockFetchWithCallback("AAPL", (result) => {
    outputDiv.innerHTML += `<p>${result}</p>`;
  });
}

function simulateStockFetchWithPromise(symbol) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (stockData[symbol]) {
        resolve(`${symbol} Price (Promise): $${stockData[symbol]}`);
      } else {reject("Stock not found");}
    }, 1000);
  }); 
}

function getStockWithPromise() {
  simulateStockFetchWithPromise("MSFT")
    .then(result => outputDiv.innerHTML += `<p>${result}</p>`)
    .catch(err => outputDiv.innerHTML = `<p style="color:red">${err}</p>`);
}

function simulateStockFetchAsync(symbol) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (stockData[symbol]) {
        resolve(`${symbol} Price (Async/Await): $${stockData[symbol]}`);
      } else {
        reject("Stock not found");
      }
    }, 1000);
  });
}

async function getStockWithAsyncAwait() {
  try {
    const result = await simulateStockFetchAsync("GOOGL");
    outputDiv.innerHTML += `<p>${result}</p>`;
  } catch (err) {
    outputDiv.innerHTML = `<p style="color:red">${err}</p>`;
  }
}