// module.exports = {
//   "/api/*": {
//     context: "/",
//     // target: process.env["services__apiReverseProxy__http__0"],
//     target: "yahoooo",
//     secure: process.env["NODE_ENV"] !== "development",
//     pathRewrite: {
//       "^/api": "",
//     },
//   },
// };
module.exports = {

  "/api": {
    "target": "http://localhost:5155",
    "secure": false,
    "changeOrigin": true,
    "pathRewrite": {
      "^/api": ""
    }
  }
}
