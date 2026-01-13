import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateAdvProcessos, useUpdateAdvProcessos } from "@/lib/abp/hooks/useAdvProcessoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProcesso: z.any(),
  
    idCliente: z.any(),
  
    idUsuarioInclusao: z.any(),
  
    idEscritorioOrigem: z.any(),
  
    idEscritorioResponsavel: z.any(),
  
    idAutorPeticao: z.any(),
  
    idResponsavel: z.any(),
  
    sintese: z.any(),
  
    numero: z.any(),
  
    dataDistribuicao: z.any(),
  
    idStatus: z.any(),
  
    idNatureza: z.any(),
  
    idTipo: z.any(),
  
    estado: z.any(),
  
    cidade: z.any(),
  
    idFase: z.any(),
  
    idRelevancia: z.any(),
  
    idProbabilidade: z.any(),
  
    valorCausa: z.any(),
  
    valorHonorarios: z.any(),
  
    valorHonorariosTipo: z.any(),
  
    observacoes: z.any(),
  
    idSentenca: z.any(),
  
    dataSentenca: z.any(),
  
    alvara: z.any(),
  
    valorDeferido: z.any(),
  
    dataEncerramento: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idOrgao: z.any(),
  
    idInstancia: z.any(),
  
    idVara: z.any(),
  
    recurso: z.any(),
  
    recursoIdSentenca: z.any(),
  
    recursoDataSentenca: z.any(),
  
    alvaraPendente: z.any(),
  
    alvaraPendenteDesde: z.any(),
  
    historicoNumeros: z.any(),
  
    recebeAcordo: z.any(),
  
    recebeRPV: z.any(),
  
    recebePrecatorio: z.any(),
  
    recebeAlvara: z.any(),
  
    recebeBanco: z.any(),
  
    recebeDataLiberacao: z.any(),
  
    pendOutrosValores: z.any(),
  
    pendOutrosValoresDataEncerramento: z.any(),
  
    pendOutrosValoresDeferido: z.any(),
  
    pendOutrosValoresValorDeferido: z.any(),
  
    acaoColetiva: z.any(),
  
    temResponsavel: z.any(),
  
    nomeResponsavel: z.any(),
  
    cpfResponsavel: z.any(),
  
    imposto: z.any(),
  
    tarifa: z.any(),
  
    complementoPositivo: z.any(),
  
    rPV: z.any(),
  
    bancarioBanco: z.any(),
  
    bancarioTipoConta: z.any(),
  
    bancarioAgencia: z.any(),
  
    bancarioConta: z.any(),
  
    bancarioFavorecido: z.any(),
  
    bancarioCpf: z.any(),
  
    nomeReu: z.any(),
  
    sucumbencia: z.any(),
  
    idConta: z.any(),
  
    dataLiberacaoValorDeferido: z.any(),
  
    boleto: z.any(),
  
    precatorio: z.any(),
  
    emitir: z.any(),
  
    emitido: z.any(),
  
    formaRecebimento: z.any(),
  
    bancarioBancoId: z.any(),
  
    dataPrevisaoRepasseCliente: z.any(),
  
    honorariosTextoFicha: z.any(),
  
    nfComComplementoPositivo: z.any(),
  
    valorHonorariosDestaque: z.any(),
  
    valorHonorariosDestaqueTipo: z.any(),
  
    dataPrevisaoHonorariosDestaque: z.any(),
  
    idContaPagar: z.any(),
  
    bancarioPerc: z.any(),
  
    dataPrevistaClienteReceber: z.any(),
  
    sucumbenciaAdd: z.any(),
  
    sucumbenciaAddData: z.any(),
  
    sucumbenciaAddIdBanco: z.any(),
  
    saldoDevedor: z.any(),
  
    herdeirosTipoValor: z.any(),
  
    nrParcelasProcesso: z.any(),
  
    nrParcelasSomenteSucumbencia: z.any(),
  
    preProcesso: z.any(),
  
    preProcessoPasta: z.any(),
  
    preProcessoDataCriacao: z.any(),
  
    preProcessoDataPrevista: z.any(),
  
    preProcessoDataRealizada: z.any(),
  
    preProcessoIdStatus: z.any(),
  
    tsConversao: z.any(),
  
    perdido: z.any(),
  
    tsPerdido: z.any(),
  
    idMotivoPerda: z.any(),
  
    convertido: z.any(),
  
    clientePrimeiraVez: z.any(),
  
    preProcessoIdTipo: z.any(),
  
    tarifaParcelas: z.any(),
  
    idOrigem: z.any(),
  
    dataEntrada: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProcessosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProcessosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProcessosFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateAdvProcessos();
const updateMutation = useUpdateAdvProcessos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProcessos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProcessos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprocessos:", error);
    toast.error(error.message || "Failed to save advprocessos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProcessos": "Create advProcessos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprocessos." : "Fill in the details to create a new advprocessos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente</Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioInclusao" > idUsuarioInclusao</Label>

<Input id="idUsuarioInclusao" type = "number" step = "any" {...register("idUsuarioInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idEscritorioOrigem" > idEscritorioOrigem</Label>

<Input id="idEscritorioOrigem" type = "number" step = "any" {...register("idEscritorioOrigem") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idEscritorioResponsavel" > idEscritorioResponsavel</Label>

<Input id="idEscritorioResponsavel" type = "number" step = "any" {...register("idEscritorioResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idAutorPeticao" > idAutorPeticao</Label>

<Input id="idAutorPeticao" type = "number" step = "any" {...register("idAutorPeticao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idResponsavel" > idResponsavel</Label>

<Input id="idResponsavel" type = "number" step = "any" {...register("idResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sintese" > sintese</Label>

<Input id="sintese" {...register("sintese") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="numero" > numero</Label>

<Input id="numero" {...register("numero") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataDistribuicao" > dataDistribuicao</Label>

<Input id="dataDistribuicao" {...register("dataDistribuicao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idStatus" > idStatus</Label>

<Input id="idStatus" type = "number" step = "any" {...register("idStatus") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idNatureza" > idNatureza</Label>

<Input id="idNatureza" type = "number" step = "any" {...register("idNatureza") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipo" > idTipo</Label>

<Input id="idTipo" type = "number" step = "any" {...register("idTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="estado" > estado</Label>

<Input id="estado" {...register("estado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cidade" > cidade</Label>

<Input id="cidade" {...register("cidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idFase" > idFase</Label>

<Input id="idFase" type = "number" step = "any" {...register("idFase") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idRelevancia" > idRelevancia</Label>

<Input id="idRelevancia" type = "number" step = "any" {...register("idRelevancia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProbabilidade" > idProbabilidade</Label>

<Input id="idProbabilidade" type = "number" step = "any" {...register("idProbabilidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorCausa" > valorCausa</Label>

<Input id="valorCausa" type = "number" step = "any" {...register("valorCausa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorHonorarios" > valorHonorarios</Label>

<Input id="valorHonorarios" type = "number" step = "any" {...register("valorHonorarios") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorHonorariosTipo" > valorHonorariosTipo</Label>

<Input id="valorHonorariosTipo" {...register("valorHonorariosTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="observacoes" > observacoes</Label>

<Input id="observacoes" {...register("observacoes") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idSentenca" > idSentenca</Label>

<Input id="idSentenca" type = "number" step = "any" {...register("idSentenca") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataSentenca" > dataSentenca</Label>

<Input id="dataSentenca" {...register("dataSentenca") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="alvara" > alvara</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="alvara"
checked = { watch("alvara") }
onCheckedChange = {(checked) => setValue("alvara", !!checked)}
              />
  < label htmlFor = "alvara" className = "text-sm font-normal" >
    { watch("alvara") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="valorDeferido" > valorDeferido</Label>

<Input id="valorDeferido" type = "number" step = "any" {...register("valorDeferido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataEncerramento" > dataEncerramento</Label>

<Input id="dataEncerramento" {...register("dataEncerramento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idOrgao" > idOrgao</Label>

<Input id="idOrgao" type = "number" step = "any" {...register("idOrgao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idInstancia" > idInstancia</Label>

<Input id="idInstancia" type = "number" step = "any" {...register("idInstancia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idVara" > idVara</Label>

<Input id="idVara" type = "number" step = "any" {...register("idVara") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recurso" > recurso</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recurso"
checked = { watch("recurso") }
onCheckedChange = {(checked) => setValue("recurso", !!checked)}
              />
  < label htmlFor = "recurso" className = "text-sm font-normal" >
    { watch("recurso") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recursoIdSentenca" > recursoIdSentenca</Label>

<Input id="recursoIdSentenca" type = "number" step = "any" {...register("recursoIdSentenca") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recursoDataSentenca" > recursoDataSentenca</Label>

<Input id="recursoDataSentenca" {...register("recursoDataSentenca") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="alvaraPendente" > alvaraPendente</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="alvaraPendente"
checked = { watch("alvaraPendente") }
onCheckedChange = {(checked) => setValue("alvaraPendente", !!checked)}
              />
  < label htmlFor = "alvaraPendente" className = "text-sm font-normal" >
    { watch("alvaraPendente") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="alvaraPendenteDesde" > alvaraPendenteDesde</Label>

<Input id="alvaraPendenteDesde" {...register("alvaraPendenteDesde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="historicoNumeros" > historicoNumeros</Label>

<Input id="historicoNumeros" {...register("historicoNumeros") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeAcordo" > recebeAcordo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebeAcordo"
checked = { watch("recebeAcordo") }
onCheckedChange = {(checked) => setValue("recebeAcordo", !!checked)}
              />
  < label htmlFor = "recebeAcordo" className = "text-sm font-normal" >
    { watch("recebeAcordo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeRPV" > recebeRPV</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebeRPV"
checked = { watch("recebeRPV") }
onCheckedChange = {(checked) => setValue("recebeRPV", !!checked)}
              />
  < label htmlFor = "recebeRPV" className = "text-sm font-normal" >
    { watch("recebeRPV") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebePrecatorio" > recebePrecatorio</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebePrecatorio"
checked = { watch("recebePrecatorio") }
onCheckedChange = {(checked) => setValue("recebePrecatorio", !!checked)}
              />
  < label htmlFor = "recebePrecatorio" className = "text-sm font-normal" >
    { watch("recebePrecatorio") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeAlvara" > recebeAlvara</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebeAlvara"
checked = { watch("recebeAlvara") }
onCheckedChange = {(checked) => setValue("recebeAlvara", !!checked)}
              />
  < label htmlFor = "recebeAlvara" className = "text-sm font-normal" >
    { watch("recebeAlvara") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeBanco" > recebeBanco</Label>

<Input id="recebeBanco" {...register("recebeBanco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recebeDataLiberacao" > recebeDataLiberacao</Label>

<Input id="recebeDataLiberacao" {...register("recebeDataLiberacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pendOutrosValores" > pendOutrosValores</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pendOutrosValores"
checked = { watch("pendOutrosValores") }
onCheckedChange = {(checked) => setValue("pendOutrosValores", !!checked)}
              />
  < label htmlFor = "pendOutrosValores" className = "text-sm font-normal" >
    { watch("pendOutrosValores") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="pendOutrosValoresDataEncerramento" > pendOutrosValoresDataEncerramento</Label>

<Input id="pendOutrosValoresDataEncerramento" {...register("pendOutrosValoresDataEncerramento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pendOutrosValoresDeferido" > pendOutrosValoresDeferido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pendOutrosValoresDeferido"
checked = { watch("pendOutrosValoresDeferido") }
onCheckedChange = {(checked) => setValue("pendOutrosValoresDeferido", !!checked)}
              />
  < label htmlFor = "pendOutrosValoresDeferido" className = "text-sm font-normal" >
    { watch("pendOutrosValoresDeferido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="pendOutrosValoresValorDeferido" > pendOutrosValoresValorDeferido</Label>

<Input id="pendOutrosValoresValorDeferido" type = "number" step = "any" {...register("pendOutrosValoresValorDeferido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="acaoColetiva" > acaoColetiva</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="acaoColetiva"
checked = { watch("acaoColetiva") }
onCheckedChange = {(checked) => setValue("acaoColetiva", !!checked)}
              />
  < label htmlFor = "acaoColetiva" className = "text-sm font-normal" >
    { watch("acaoColetiva") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="temResponsavel" > temResponsavel</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="temResponsavel"
checked = { watch("temResponsavel") }
onCheckedChange = {(checked) => setValue("temResponsavel", !!checked)}
              />
  < label htmlFor = "temResponsavel" className = "text-sm font-normal" >
    { watch("temResponsavel") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="nomeResponsavel" > nomeResponsavel</Label>

<Input id="nomeResponsavel" {...register("nomeResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cpfResponsavel" > cpfResponsavel</Label>

<Input id="cpfResponsavel" {...register("cpfResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="imposto" > imposto</Label>

<Input id="imposto" type = "number" step = "any" {...register("imposto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tarifa" > tarifa</Label>

<Input id="tarifa" type = "number" step = "any" {...register("tarifa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="complementoPositivo" > complementoPositivo</Label>

<Input id="complementoPositivo" type = "number" step = "any" {...register("complementoPositivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="rPV" > RPV</Label>

<Input id="rPV" {...register("rPV") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioBanco" > bancarioBanco</Label>

<Input id="bancarioBanco" {...register("bancarioBanco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioTipoConta" > bancarioTipoConta</Label>

<Input id="bancarioTipoConta" {...register("bancarioTipoConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioAgencia" > bancarioAgencia</Label>

<Input id="bancarioAgencia" {...register("bancarioAgencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioConta" > bancarioConta</Label>

<Input id="bancarioConta" {...register("bancarioConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioFavorecido" > bancarioFavorecido</Label>

<Input id="bancarioFavorecido" {...register("bancarioFavorecido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioCpf" > bancarioCpf</Label>

<Input id="bancarioCpf" {...register("bancarioCpf") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nomeReu" > nomeReu</Label>

<Input id="nomeReu" {...register("nomeReu") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sucumbencia" > sucumbencia</Label>

<Input id="sucumbencia" type = "number" step = "any" {...register("sucumbencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idConta" > idConta</Label>

<Input id="idConta" type = "number" step = "any" {...register("idConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataLiberacaoValorDeferido" > dataLiberacaoValorDeferido</Label>

<Input id="dataLiberacaoValorDeferido" {...register("dataLiberacaoValorDeferido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="boleto" > boleto</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="boleto"
checked = { watch("boleto") }
onCheckedChange = {(checked) => setValue("boleto", !!checked)}
              />
  < label htmlFor = "boleto" className = "text-sm font-normal" >
    { watch("boleto") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="precatorio" > precatorio</Label>

<Input id="precatorio" {...register("precatorio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="emitir" > emitir</Label>

<Input id="emitir" {...register("emitir") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="emitido" > emitido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="emitido"
checked = { watch("emitido") }
onCheckedChange = {(checked) => setValue("emitido", !!checked)}
              />
  < label htmlFor = "emitido" className = "text-sm font-normal" >
    { watch("emitido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="formaRecebimento" > formaRecebimento</Label>

<Input id="formaRecebimento" {...register("formaRecebimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioBancoId" > bancarioBancoId</Label>

<Input id="bancarioBancoId" type = "number" step = "any" {...register("bancarioBancoId") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevisaoRepasseCliente" > dataPrevisaoRepasseCliente</Label>

<Input id="dataPrevisaoRepasseCliente" {...register("dataPrevisaoRepasseCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="honorariosTextoFicha" > honorariosTextoFicha</Label>

<Input id="honorariosTextoFicha" {...register("honorariosTextoFicha") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nfComComplementoPositivo" > nfComComplementoPositivo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="nfComComplementoPositivo"
checked = { watch("nfComComplementoPositivo") }
onCheckedChange = {(checked) => setValue("nfComComplementoPositivo", !!checked)}
              />
  < label htmlFor = "nfComComplementoPositivo" className = "text-sm font-normal" >
    { watch("nfComComplementoPositivo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="valorHonorariosDestaque" > valorHonorariosDestaque</Label>

<Input id="valorHonorariosDestaque" type = "number" step = "any" {...register("valorHonorariosDestaque") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorHonorariosDestaqueTipo" > valorHonorariosDestaqueTipo</Label>

<Input id="valorHonorariosDestaqueTipo" {...register("valorHonorariosDestaqueTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevisaoHonorariosDestaque" > dataPrevisaoHonorariosDestaque</Label>

<Input id="dataPrevisaoHonorariosDestaque" {...register("dataPrevisaoHonorariosDestaque") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idContaPagar" > idContaPagar</Label>

<Input id="idContaPagar" type = "number" step = "any" {...register("idContaPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioPerc" > bancarioPerc</Label>

<Input id="bancarioPerc" type = "number" step = "any" {...register("bancarioPerc") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevistaClienteReceber" > dataPrevistaClienteReceber</Label>

<Input id="dataPrevistaClienteReceber" {...register("dataPrevistaClienteReceber") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sucumbenciaAdd" > sucumbenciaAdd</Label>

<Input id="sucumbenciaAdd" type = "number" step = "any" {...register("sucumbenciaAdd") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sucumbenciaAddData" > sucumbenciaAddData</Label>

<Input id="sucumbenciaAddData" {...register("sucumbenciaAddData") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sucumbenciaAddIdBanco" > sucumbenciaAddIdBanco</Label>

<Input id="sucumbenciaAddIdBanco" type = "number" step = "any" {...register("sucumbenciaAddIdBanco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="saldoDevedor" > saldoDevedor</Label>

<Input id="saldoDevedor" type = "number" step = "any" {...register("saldoDevedor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="herdeirosTipoValor" > herdeirosTipoValor</Label>

<Input id="herdeirosTipoValor" {...register("herdeirosTipoValor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nrParcelasProcesso" > nrParcelasProcesso</Label>

<Input id="nrParcelasProcesso" type = "number" step = "any" {...register("nrParcelasProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nrParcelasSomenteSucumbencia" > nrParcelasSomenteSucumbencia</Label>

<Input id="nrParcelasSomenteSucumbencia" type = "number" step = "any" {...register("nrParcelasSomenteSucumbencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcesso" > preProcesso</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="preProcesso"
checked = { watch("preProcesso") }
onCheckedChange = {(checked) => setValue("preProcesso", !!checked)}
              />
  < label htmlFor = "preProcesso" className = "text-sm font-normal" >
    { watch("preProcesso") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcessoPasta" > preProcessoPasta</Label>

<Input id="preProcessoPasta" {...register("preProcessoPasta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcessoDataCriacao" > preProcessoDataCriacao</Label>

<Input id="preProcessoDataCriacao" {...register("preProcessoDataCriacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcessoDataPrevista" > preProcessoDataPrevista</Label>

<Input id="preProcessoDataPrevista" {...register("preProcessoDataPrevista") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcessoDataRealizada" > preProcessoDataRealizada</Label>

<Input id="preProcessoDataRealizada" {...register("preProcessoDataRealizada") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcessoIdStatus" > preProcessoIdStatus</Label>

<Input id="preProcessoIdStatus" type = "number" step = "any" {...register("preProcessoIdStatus") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsConversao" > tsConversao</Label>

<Input id="tsConversao" type = "date" {...register("tsConversao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="perdido" > perdido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="perdido"
checked = { watch("perdido") }
onCheckedChange = {(checked) => setValue("perdido", !!checked)}
              />
  < label htmlFor = "perdido" className = "text-sm font-normal" >
    { watch("perdido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsPerdido" > tsPerdido</Label>

<Input id="tsPerdido" type = "date" {...register("tsPerdido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idMotivoPerda" > idMotivoPerda</Label>

<Input id="idMotivoPerda" type = "number" step = "any" {...register("idMotivoPerda") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="convertido" > convertido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="convertido"
checked = { watch("convertido") }
onCheckedChange = {(checked) => setValue("convertido", !!checked)}
              />
  < label htmlFor = "convertido" className = "text-sm font-normal" >
    { watch("convertido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="clientePrimeiraVez" > clientePrimeiraVez</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="clientePrimeiraVez"
checked = { watch("clientePrimeiraVez") }
onCheckedChange = {(checked) => setValue("clientePrimeiraVez", !!checked)}
              />
  < label htmlFor = "clientePrimeiraVez" className = "text-sm font-normal" >
    { watch("clientePrimeiraVez") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="preProcessoIdTipo" > preProcessoIdTipo</Label>

<Input id="preProcessoIdTipo" type = "number" step = "any" {...register("preProcessoIdTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tarifaParcelas" > tarifaParcelas</Label>

<Input id="tarifaParcelas" {...register("tarifaParcelas") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idOrigem" > idOrigem</Label>

<Input id="idOrigem" type = "number" step = "any" {...register("idOrigem") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataEntrada" > dataEntrada</Label>

<Input id="dataEntrada" {...register("dataEntrada") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
