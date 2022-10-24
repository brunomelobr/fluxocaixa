using BrunoMelo.FluxoCaixa.Client.Services.Manutencao;
using BrunoMelo.FluxoCaixa.Client.Services.Operacional;
using BrunoMelo.FluxoCaixa.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NuvTools.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Operacional;

public partial class TransacaoEdicaoModal
{
    [Inject] TransacaoService Service { get; set; }
    [Inject] CategoriaService CategoriaService { get; set; }
    [Inject] ContaService ContaService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public long? Id { get; set; }

    public EditContext EditContext { get; set; }

    public class ObjectView
    {
        [ValidateComplexType]
        public FluxoCaixa.Models.Data.Operacional.Transacao Data { get; set; }

        [Display(Name = nameof(Fields.Date), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        public DateTime? DataNulo
        {
            get
            {
                return (Data.Data == DateTime.MinValue) ? null : Data.Data;
            }
            set
            {
                Data.Data = (value == null) ? DateTime.MinValue : value.Value;
            }
        }

        [Display(Name = nameof(Fields.Value), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        public decimal? ValorNulo
        {
            get
            {
                return (Data.Valor == 0) ? null : Data.Valor;
            }
            set
            {
                Data.Valor = (value == null) ? 0 : value.Value;
            }
        }

        [Display(Name = nameof(Fields.Type), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        public short? TipoTransacaoIdNulo
        {
            get
            {
                return (Data.TipoTransacaoId == 0) ? null : Data.TipoTransacaoId;
            }
            set
            {
                Data.TipoTransacaoId = (value == null) ? (short)0 : value.Value;
            }
        }

        [Display(Name = nameof(CampoEspecifico.Conta), ResourceType = typeof(CampoEspecifico))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        public int? ContaIdNulo
        {
            get
            {
                return (Data.ContaId == 0) ? null : Data.ContaId;
            }
            set
            {
                Data.ContaId = (value == null) ? 0 : value.Value;
            }
        }

        [Display(Name = nameof(CampoEspecifico.Categoria), ResourceType = typeof(CampoEspecifico))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        public int? CategoriaIdNulo
        {
            get
            {
                return (Data.CategoriaId == 0) ? null : Data.CategoriaId;
            }
            set
            {
                Data.CategoriaId = (value == null) ? 0 : value.Value;
            }
        }
    }

    public ObjectView ObjetoEdicao = new() { Data = new() { Categoria = new(), Conta = new() } };

    private IEnumerable<FluxoCaixa.Models.Data.Apoio.TipoTransacao> ListaTipoTransacao { get; set; }

    private IEnumerable<FluxoCaixa.Models.Data.Manutencao.Categoria> ListaCategorias { get; set; }
    private IEnumerable<FluxoCaixa.Models.Data.Manutencao.Conta> ListaContas { get; set; }

    private bool ExibeConfirmacaoExclusao { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        EditContext = new(ObjetoEdicao);

        if (!(await _stateProvider.GetAuthenticationStateProviderUserAsync()).Identity.IsAuthenticated)
        {
            _navigationManager.NavigateTo("/login");
            return;
        }

        ListaTipoTransacao = NuvTools.Common.Enums.Enumeration.GetList<FluxoCaixa.Models.Util.Enumeracao.TipoTransacao,
                                                            FluxoCaixa.Models.Data.Apoio.TipoTransacao, short>("TipoTransacaoId", "Nome");

        ListaCategorias = await CategoriaService.ConsultarAsync();
        ListaContas = await ContaService.ConsultarAsync();

        if (Id != null && Id.HasValue)
        {
            FluxoCaixa.Models.Data.Operacional.Transacao objeto = await Service.ConsultarAsync(Id.Value);

            ObjetoEdicao.Data = objeto;
            EditContext = new(ObjetoEdicao);
        }
    }

    public void Cancelar()
    {
        MudDialog.Cancel();
    }

    private async Task ExcluirAsync()
    {
        ExibirMensagemResultado(await Service.ExcluirAsync(ObjetoEdicao.Data.TransacaoId));
    }

    private async Task SalvarAsync()
    {
        if (!EditContext.Validate()) return;

        ExibirMensagemResultado(await Service.SalvarAsync(ObjetoEdicao.Data));
    }

    private void ExibirMensagemResultado(NuvTools.Common.ResultWrapper.IResult response)
    {
        if (response.Succeeded)
        {
            _snackBar.Add(_localizerMessages[nameof(Messages.OperationPerformedSuccessfully)], Severity.Success);
            MudDialog.Close();
        }
        else
        {
            string erro = _localizerMessages[nameof(Messages.TheOperationCouldNotBePerformed)];

            foreach (var message in response.Messages)
            {
                erro += "\n" + message;
            }

            _snackBar.Add(erro, Severity.Error);
        }
    }
}