using JogoGourmet.Application.DTOs;
using JogoGourmet.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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

        private void btnIniciarJogo_Click(object sender, EventArgs e)
        {
            AdivinharPratos();
        }

        private async void AdivinharPratos()
        {
            /*ponto de quebra (onde o novo prato deverá ser inserido caso necessario)*/
            var nodeAtual = arvorePratos;
            var nodeAnterior = arvorePratos;

            /// Resposta do questionamento
            DialogResult resposta;

            bool validaComCaracteristica = false;

            

            /*percorro a arvore de pratos perguntando sobre suas caracteristicas*/
            while(nodeAtual.NodeSemCaracteristicaId != null || nodeAtual.NodeComCaracteristicaId != null)
            {
                nodeAtual.Prato = await _pratoAppService.ObterPorId(nodeAtual.PratoId);
                resposta = AdivinharCaracteristica(nodeAtual.Prato);

                if (nodeAtual.Id == arvorePratos.Id && resposta == DialogResult.Yes)
                {
                    validaComCaracteristica = true;
                }

                if (resposta == DialogResult.Yes && nodeAtual.NodeComCaracteristicaId != null)
                {
                    nodeAnterior = nodeAtual;
                    nodeAtual = await _arvorePratoAppService.ObterPorId((Guid)nodeAtual.NodeComCaracteristicaId);
                } 
                   
                else if (resposta == DialogResult.No && nodeAtual.NodeSemCaracteristicaId != null)
                {
                    nodeAnterior = nodeAtual;
                    nodeAtual = await _arvorePratoAppService.ObterPorId((Guid)nodeAtual.NodeSemCaracteristicaId);
                }  
                else
                    break;

                
            }

            nodeAtual.Prato = await _pratoAppService.ObterPorId(nodeAtual.PratoId);
            resposta =  AdivinharPrato(validaComCaracteristica ? nodeAnterior.Prato : nodeAtual.Prato);

            /* se adivinhou o prato*/
            if (resposta == DialogResult.Yes)
            {
                MessageBox.Show("Acertei de novo!", "", MessageBoxButtons.OK);
            }
            else
            {
                AdicionarNovoPrato(nodeAnterior, nodeAtual, validaComCaracteristica); /*se a caracteristica foi questionada e ele possui ela adicionar depois, senão, antes*/
            }     
        }

        private DialogResult AdivinharCaracteristica(PratoDto prato)
        {
            return MessageBox.Show("O Prato que você pensou é " + prato.Caracteristica + "?", "", MessageBoxButtons.YesNo);
        }

        private DialogResult AdivinharPrato(PratoDto prato)
        {
            return MessageBox.Show("O Prato que você pensou é " + prato.Nome + "?", "", MessageBoxButtons.YesNo);
        }

        private async void AdicionarNovoPrato(ArvorePratoDto nodePai, ArvorePratoDto node, bool comCaracteristica)
        {
            var nomePrato = Microsoft.VisualBasic.Interaction.InputBox("Qual prato você pensou?", "Desisto", string.Empty);

            var caracteristicaPrato = Microsoft.VisualBasic.Interaction.InputBox(nomePrato + " é _____ mas " + (comCaracteristica ? nodePai.Prato.Nome : node.Prato.Nome) + " não.", "Complete", string.Empty);

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
                ArvorePratoDto novoNode;

                if (comCaracteristica)
                {
                    novoNode = new ArvorePratoDto()
                    {
                        PratoId = novoPrato.Id,
                        NodeSemCaracteristicaId = null,
                        NodeComCaracteristicaId = node.Id
                    };
                }
                else
                {
                    novoNode = new ArvorePratoDto()
                    {
                        PratoId = novoPrato.Id,
                        NodeSemCaracteristicaId = node.Id,
                        NodeComCaracteristicaId = null
                    };
                }

                await _arvorePratoAppService.Adicionar(novoNode);

                /*Aponto o node pai pro novo node*/
                if (comCaracteristica)
                    nodePai.NodeComCaracteristicaId = novoNode.Id;
                else
                    nodePai.NodeSemCaracteristicaId = novoNode.Id;

                await _arvorePratoAppService.Alterar(nodePai);
            }
        }
    }
}
