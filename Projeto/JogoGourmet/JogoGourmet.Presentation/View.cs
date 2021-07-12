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

            var pratoComCaracteristicaInicial = new PratoDto() { Nome = "Lasanha", Caracteristica = null };

            await _pratoAppService.Adicionar(pratoComCaracteristicaInicial);

            /*criando arvore*/
            arvorePratos = new ArvorePratoDto();

            arvorePratos.SetPrato(caracteristicaInicial);


            var nodeSemCaracteristica = new ArvorePratoDto();

            nodeSemCaracteristica.SetPrato(pratoSemCaracteristicaInicial);

            await _arvorePratoAppService.Adicionar(nodeSemCaracteristica);

            var nodeComCaracteristica = new ArvorePratoDto();

            nodeComCaracteristica.SetPrato(pratoComCaracteristicaInicial);

            await _arvorePratoAppService.Adicionar(nodeComCaracteristica);


            arvorePratos.SetNodeSemCaracteristica(nodeSemCaracteristica);
            arvorePratos.SetNodeComCaracteristica(nodeComCaracteristica);

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

            /*percorro a arvore de pratos perguntando sobre suas caracteristicas*/
            while(node.NodeSemCaracteristicaId != null || node.NodeComCaracteristicaId != null)
            {
                node.Prato = await _pratoAppService.ObterPorId(node.PratoId);
                resposta = AdivinharCaracteristica(node.Prato);

                if (resposta == DialogResult.No && node.NodeSemCaracteristicaId != null)
                {
                    nodePai = node;
                    node = await _arvorePratoAppService.ObterPorId((Guid)node.NodeSemCaracteristicaId);
                    node.Prato = await _pratoAppService.ObterPorId(node.PratoId);
                    return await AdivinharPratos(nodePai, node, finalizou);
                }
                else if (resposta == DialogResult.Yes && node.NodeComCaracteristicaId != null)
                {
                    nodePai = node;
                    node = await _arvorePratoAppService.ObterPorId((Guid)node.NodeComCaracteristicaId);
                    node.Prato = await _pratoAppService.ObterPorId(node.PratoId);
                    return await AdivinharPratos(nodePai, node, finalizou);
                }
                else
                    break;
            }

            node.Prato = await _pratoAppService.ObterPorId(node.PratoId);
            resposta =  AdivinharPrato(node.Prato);

            /* se adivinhou o prato*/
            if (resposta == DialogResult.Yes)
            {
                MessageBox.Show("Acertei de novo!", "", MessageBoxButtons.OK);
            }
            else
            {
                AdicionarNovoPrato(nodePai, node);
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

        private async void AdicionarNovoPrato(ArvorePratoDto nodePai, ArvorePratoDto node)
        {
            var nomePrato = Microsoft.VisualBasic.Interaction.InputBox("Qual prato você pensou?", "Desisto", string.Empty);

            var caracteristicaPrato = Microsoft.VisualBasic.Interaction.InputBox(nomePrato + " é _____ mas " + (node.Prato.Nome) + " não.", "Complete", string.Empty);

            if (nomePrato.Trim() == string.Empty || nomePrato.Trim() == string.Empty)
                MessageBox.Show("Não consegui identificar o novo prato, que pena!", "", MessageBoxButtons.OK);
            else
            {
                /*crio novo prato*/
                var novoPrato = new PratoDto()
                {
                    Nome = nomePrato,
                    Caracteristica = caracteristicaPrato
                };

                await _pratoAppService.Adicionar(novoPrato);

                /*crio novo node*/
                var novoNode = new ArvorePratoDto(){
                        PratoId = novoPrato.Id,
                        NodeSemCaracteristicaId = node.Id,
                        NodeComCaracteristicaId = null
                };
                
                await _arvorePratoAppService.Adicionar(novoNode);

                /*Aponto o node pai pro novo node*/
                if (nodePai.NodeSemCaracteristicaId == node.Id)
                {
                    nodePai.NodeSemCaracteristicaId = novoNode.Id;
                }
                else
                {
                    nodePai.NodeComCaracteristicaId = novoNode.Id;
                }

                await _arvorePratoAppService.Alterar(nodePai);
            }
        }
    }
}
