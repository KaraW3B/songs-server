var defaultTarget = 'http://localhost:7373';
module.exports = [
{
   context: ['/api/**'],
   pathRewrite: {'/api' : ''},
   target: defaultTarget,
   secure: false,
}
];