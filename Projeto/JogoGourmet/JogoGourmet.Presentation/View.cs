using JogoGourmet.Application.DTOs;
using JogoGourmet.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoGourmet.Presentation
{
    public partial class View : Form
    {
        private ArvorePratoDto arvorePratos;

        private readonly IPratoAppService _pratoAppService = new PratoAppService();

        private readonly IArvorePratoAppService _arvorePratoAppService = new ArvorePratoAppService();
        public View()
        {
            InitializeComponent();

            InicializarCache();
        }

        private async void InicializarCache()
        {
            /*definindo parametros iniciais*/
            var caracteristicaInicial = new PratoDto() { Nome = "Lasanha", Caracteristica = "massa" };
            await _pratoAppService.Adicionar(caracteristicaInicial);

            var pratoSemCaracteristicaInicial = new PratoDto() { Nome = "Bolo de chocolate", Caracteristica = null };
            await _pratoAppService.Adicionar(pratoSemCaracteristicaInicial);

            /*criando arvore*/
            arvorePratos = new ArvorePratoDto();
            arvorePratos.SetPrato(caracteristicaInicial);

            var nodeSemCaracteristica = new ArvorePratoDto();
            nodeSemCaracteristica.SetPrato(pratoSemCaracteristicaInicial);
            await _arvorePratoAppService.Adicionar(nodeSemCaracteristica);

            arvorePratos.SetNodeSemCaracteristica(nodeSemCaracteristica);
            await _arvorePratoAppService.Adicionar(arvorePratos);
        }

        private async void btnIniciarJogo_Click(object sender, EventArgs e)
        {
            arvorePratos = await _arvorePratoAppService.ObterPorId(arvorePratos.Id);

            await AdivinharPratos(arvorePratos, arvorePratos, false);
        }

        private async Task<bool> AdivinharPratos(ArvorePratoDto nodePai, ArvorePratoDto node, bool finalizou)
        {
            if (finalizou)
                return true;

            DialogResult resposta;

            bool comCaracteristica = false;

            node.Prato = await _pratoAppService.ObterPorId(node.PratoId);

            if (node.Prato.Caracteristica != null || node.NodeSemCaracteristicaId != null || node.NodeComCaracteristicaId != null)
            {
                resposta = AdivinharCaracteristica(node.Prato);
                comCaracteristica = (resposta == DialogResult.Yes);

                if (resposta == DialogResult.No && node.NodeSemCaracteristicaId != null)
                {
                    nodePai = node;
                    node = await _arvorePratoAppService.ObterPorId((Guid)node.NodeSemCaracteristicaId);
                    return await AdivinharPratos(nodePai, node, finalizou);
                }
                else if (resposta == DialogResult.Yes && node.NodeComCaracteristicaId != null)
                {
                    nodePai = node;
                    node = await _arvorePratoAppService.ObterPorId((Guid)node.NodeComCaracteristicaId);
                    return await AdivinharPratos(nodePai, node, finalizou);
                }
            }
  
            /*se não for ultimo prato(bolo de chocolate) e nao tiver a caracteristca, pergunto em relação ao prato anterior*/
            resposta =  AdivinharPrato((node.Prato.Caracteristica != null && !comCaracteristica) ? nodePai.Prato : node.Prato);

            if (resposta == DialogResult.Yes)
            {
                MessageBox.Show("Acertei de novo!", "", MessageBoxButtons.OK);
            }
            else
            {
                var novoPrato = await RebecerNovoPrato(node.Prato);
                /*se for ultimo prato(bolo de chocolate) adicionar node anterior*/
                AdicionarNovoPrato(node.Prato.Caracteristica == null ? nodePai : node, novoPrato, comCaracteristica);
            }

            return true;
        }

        private DialogResult AdivinharCaracteristica(PratoDto prato)
        {
            return MessageBox.Show("O Prato que você pensou é " + prato.Caracteristica + "?", "", MessageBoxButtons.YesNo);
        }

        private DialogResult AdivinharPrato(PratoDto prato)
        {
            return MessageBox.Show("O Prato que você pensou é " + prato.Nome + "?", "", MessageBoxButtons.YesNo);
        }

        private void AdicionarNovoPrato(ArvorePratoDto node, PratoDto prato, bool adicionarComCaracteristica)
        {
            if (adicionarComCaracteristica)
                AdicionarPratoComCaracteristica(node, prato);
            else
                AdicionarPratoSemCaracteristica(node, prato);
        }

        private async Task<PratoDto> RebecerNovoPrato(PratoDto prato)
        {
            var nomePrato = Microsoft.VisualBasic.Interaction.InputBox("Qual prato você pensou?", "Desisto", string.Empty);
            var caracteristicaPrato = Microsoft.VisualBasic.Interaction.InputBox(nomePrato + " é _____ mas " + (prato.Nome) + " não.", "Complete", string.Empty);

            var novoPrato = new PratoDto()
            {
                Nome = nomePrato,
                Caracteristica = caracteristicaPrato
            };
            await _pratoAppService.Adicionar(novoPrato);

            return novoPrato;
        }

        private async void AdicionarPratoComCaracteristica(ArvorePratoDto node, PratoDto prato)
        {
            var novoNode = new ArvorePratoDto()
            {
                NodeComCaracteristicaId = node.NodeComCaracteristicaId
            };
            novoNode.SetPrato(prato);
            await _arvorePratoAppService.Adicionar(novoNode);

            node.NodeComCaracteristicaId = novoNode.Id;
            await _arvorePratoAppService.Alterar(node);
        }

        private async void AdicionarPratoSemCaracteristica(ArvorePratoDto node, PratoDto prato)
        {
            var novoNode = new ArvorePratoDto()
            {
                NodeSemCaracteristicaId = node.NodeSemCaracteristicaId
            };
            novoNode.SetPrato(prato);
            await _arvorePratoAppService.Adicionar(novoNode);

            node.NodeSemCaracteristicaId = novoNode.Id;
            await _arvorePratoAppService.Alterar(node);
        }
    }
}
