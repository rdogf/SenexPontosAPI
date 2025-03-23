namespace SenexPontosAPI.Business
{
    public class AppManagers
    {
        public EmpresaManager empresaManager { get; }
        public PessoaManager pessoaManager { get; }
        public ConsumoManager consumoManager { get; }
        public PontosManager pontosManager { get; }

        public AppManagers(
            EmpresaManager empresaManager,
            PessoaManager pessoaManager,
            ConsumoManager consumoManager,
            PontosManager pontosManager)
        {
            this.empresaManager = empresaManager;
            this.pessoaManager = pessoaManager;
            this.consumoManager = consumoManager;
            this.pontosManager = pontosManager;
        }
    }

}
