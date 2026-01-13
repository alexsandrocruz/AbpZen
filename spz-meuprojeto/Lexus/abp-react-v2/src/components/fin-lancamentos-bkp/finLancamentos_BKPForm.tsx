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
  import { useCreateFinLancamentos_BKP, useUpdateFinLancamentos_BKP } from "@/lib/abp/hooks/useFinLancamentos_BKPs";
import { toast } from "sonner";

const formSchema = z.object({
  
    idLancamento: z.any(),
  
    idConta: z.any(),
  
    idPlanoConta: z.any(),
  
    idCentroCusto: z.any(),
  
    operacao: z.any(),
  
    idForma: z.any(),
  
    modulo: z.any(),
  
    idCadastro: z.any(),
  
    idPedido: z.any(),
  
    descricao: z.any(),
  
    nrDocumento: z.any(),
  
    valor: z.any(),
  
    dataEmissao: z.any(),
  
    dataVencimento: z.any(),
  
    dataQuitacao: z.any(),
  
    quitado: z.any(),
  
    recorrente: z.any(),
  
    recorrenteChave: z.any(),
  
    previsao: z.any(),
  
    cobrancaEnviada: z.any(),
  
    parcelado: z.any(),
  
    identificacao: z.any(),
  
    observacao: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idUsuarioInclusao: z.any(),
  
    idUsuarioAlteracao: z.any(),
  
    parcela: z.any(),
  
    parcelaMaxima: z.any(),
  
    dataVencimentoOriginal: z.any(),
  
    pagtoLiberado: z.any(),
  
    dataParaPrevisao: z.any(),
  
    recorrenteVencendoVisto: z.any(),
  
    recebimentoFuturo: z.any(),
  
    recebimentoFuturoRel: z.any(),
  
    idTerceiro: z.any(),
  
    arquivoDocumento: z.any(),
  
    arquivoComprovante: z.any(),
  
    idClientePagar: z.any(),
  
    idProcessoPagar: z.any(),
  
    idArea: z.any(),
  
    identificacaoPagar: z.any(),
  
    identificacaoPagar2: z.any(),
  
    arquivoDocumento2: z.any(),
  
    arquivoComprovante2: z.any(),
  
    verba: z.any(),
  
    verbaDataDe: z.any(),
  
    verbaDataAte: z.any(),
  
    verbaEstado: z.any(),
  
    verbaCidade: z.any(),
  
    idCentroResultado: z.any(),
  
    secundaria: z.any(),
  
    geradoPeloProcesso: z.any(),
  
    sequenciaHerdeiro: z.any(),
  
    idUnidade: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinLancamentos_BKPFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinLancamentos_BKPForm({
  isOpen,
  onClose,
  initialValues,
}: FinLancamentos_BKPFormProps) {
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

  const createMutation = useCreateFinLancamentos_BKP();
const updateMutation = useUpdateFinLancamentos_BKP();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finLancamentos_BKP updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finLancamentos_BKP created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finlancamentos_bkp:", error);
    toast.error(error.message || "Failed to save finlancamentos_bkp");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finLancamentos_BKP": "Create finLancamentos_BKP" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finlancamentos_bkp." : "Fill in the details to create a new finlancamentos_bkp." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idLancamento" > idLancamento</Label>

<Input id="idLancamento" type = "number" step = "any" {...register("idLancamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idConta" > idConta * </Label>

<Input id="idConta" type = "number" step = "any" {...register("idConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPlanoConta" > idPlanoConta * </Label>

<Input id="idPlanoConta" type = "number" step = "any" {...register("idPlanoConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCentroCusto" > idCentroCusto * </Label>

<Input id="idCentroCusto" type = "number" step = "any" {...register("idCentroCusto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="operacao" > operacao</Label>

<Input id="operacao" {...register("operacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idForma" > idForma * </Label>

<Input id="idForma" type = "number" step = "any" {...register("idForma") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="modulo" > modulo</Label>

<Input id="modulo" {...register("modulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCadastro" > idCadastro</Label>

<Input id="idCadastro" type = "number" step = "any" {...register("idCadastro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPedido" > idPedido</Label>

<Input id="idPedido" type = "number" step = "any" {...register("idPedido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nrDocumento" > nrDocumento</Label>

<Input id="nrDocumento" {...register("nrDocumento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valor" > valor * </Label>

<Input id="valor" type = "number" step = "any" {...register("valor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataEmissao" > dataEmissao</Label>

<Input id="dataEmissao" {...register("dataEmissao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataVencimento" > dataVencimento</Label>

<Input id="dataVencimento" {...register("dataVencimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataQuitacao" > dataQuitacao</Label>

<Input id="dataQuitacao" {...register("dataQuitacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="quitado" > quitado * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="quitado"
checked = { watch("quitado") }
onCheckedChange = {(checked) => setValue("quitado", !!checked)}
              />
  < label htmlFor = "quitado" className = "text-sm font-normal" >
    { watch("quitado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recorrente" > recorrente * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recorrente"
checked = { watch("recorrente") }
onCheckedChange = {(checked) => setValue("recorrente", !!checked)}
              />
  < label htmlFor = "recorrente" className = "text-sm font-normal" >
    { watch("recorrente") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recorrenteChave" > recorrenteChave</Label>

<Input id="recorrenteChave" {...register("recorrenteChave") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="previsao" > previsao * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="previsao"
checked = { watch("previsao") }
onCheckedChange = {(checked) => setValue("previsao", !!checked)}
              />
  < label htmlFor = "previsao" className = "text-sm font-normal" >
    { watch("previsao") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="cobrancaEnviada" > cobrancaEnviada</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="cobrancaEnviada"
checked = { watch("cobrancaEnviada") }
onCheckedChange = {(checked) => setValue("cobrancaEnviada", !!checked)}
              />
  < label htmlFor = "cobrancaEnviada" className = "text-sm font-normal" >
    { watch("cobrancaEnviada") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="parcelado" > parcelado * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="parcelado"
checked = { watch("parcelado") }
onCheckedChange = {(checked) => setValue("parcelado", !!checked)}
              />
  < label htmlFor = "parcelado" className = "text-sm font-normal" >
    { watch("parcelado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="identificacao" > identificacao * </Label>

<Input id="identificacao" type = "number" step = "any" {...register("identificacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="observacao" > observacao</Label>

<Input id="observacao" {...register("observacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo * </Label>

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
  <Label htmlFor="tsInclusao" > tsInclusao * </Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioInclusao" > idUsuarioInclusao</Label>

<Input id="idUsuarioInclusao" type = "number" step = "any" {...register("idUsuarioInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioAlteracao" > idUsuarioAlteracao</Label>

<Input id="idUsuarioAlteracao" type = "number" step = "any" {...register("idUsuarioAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="parcela" > parcela * </Label>

<Input id="parcela" type = "number" step = "any" {...register("parcela") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="parcelaMaxima" > parcelaMaxima * </Label>

<Input id="parcelaMaxima" type = "number" step = "any" {...register("parcelaMaxima") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataVencimentoOriginal" > dataVencimentoOriginal</Label>

<Input id="dataVencimentoOriginal" {...register("dataVencimentoOriginal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pagtoLiberado" > pagtoLiberado * </Label>

<Input id="pagtoLiberado" type = "number" step = "any" {...register("pagtoLiberado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataParaPrevisao" > dataParaPrevisao</Label>

<Input id="dataParaPrevisao" {...register("dataParaPrevisao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="recorrenteVencendoVisto" > recorrenteVencendoVisto * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recorrenteVencendoVisto"
checked = { watch("recorrenteVencendoVisto") }
onCheckedChange = {(checked) => setValue("recorrenteVencendoVisto", !!checked)}
              />
  < label htmlFor = "recorrenteVencendoVisto" className = "text-sm font-normal" >
    { watch("recorrenteVencendoVisto") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebimentoFuturo" > recebimentoFuturo * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebimentoFuturo"
checked = { watch("recebimentoFuturo") }
onCheckedChange = {(checked) => setValue("recebimentoFuturo", !!checked)}
              />
  < label htmlFor = "recebimentoFuturo" className = "text-sm font-normal" >
    { watch("recebimentoFuturo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="recebimentoFuturoRel" > recebimentoFuturoRel * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="recebimentoFuturoRel"
checked = { watch("recebimentoFuturoRel") }
onCheckedChange = {(checked) => setValue("recebimentoFuturoRel", !!checked)}
              />
  < label htmlFor = "recebimentoFuturoRel" className = "text-sm font-normal" >
    { watch("recebimentoFuturoRel") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="idTerceiro" > idTerceiro</Label>

<Input id="idTerceiro" type = "number" step = "any" {...register("idTerceiro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivoDocumento" > arquivoDocumento</Label>

<Input id="arquivoDocumento" {...register("arquivoDocumento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivoComprovante" > arquivoComprovante</Label>

<Input id="arquivoComprovante" {...register("arquivoComprovante") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idClientePagar" > idClientePagar</Label>

<Input id="idClientePagar" type = "number" step = "any" {...register("idClientePagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcessoPagar" > idProcessoPagar</Label>

<Input id="idProcessoPagar" type = "number" step = "any" {...register("idProcessoPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idArea" > idArea</Label>

<Input id="idArea" type = "number" step = "any" {...register("idArea") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="identificacaoPagar" > identificacaoPagar</Label>

<Input id="identificacaoPagar" {...register("identificacaoPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="identificacaoPagar2" > identificacaoPagar2</Label>

<Input id="identificacaoPagar2" {...register("identificacaoPagar2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivoDocumento2" > arquivoDocumento2</Label>

<Input id="arquivoDocumento2" {...register("arquivoDocumento2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivoComprovante2" > arquivoComprovante2</Label>

<Input id="arquivoComprovante2" {...register("arquivoComprovante2") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="verba" > verba</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="verba"
checked = { watch("verba") }
onCheckedChange = {(checked) => setValue("verba", !!checked)}
              />
  < label htmlFor = "verba" className = "text-sm font-normal" >
    { watch("verba") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="verbaDataDe" > verbaDataDe</Label>

<Input id="verbaDataDe" {...register("verbaDataDe") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="verbaDataAte" > verbaDataAte</Label>

<Input id="verbaDataAte" {...register("verbaDataAte") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="verbaEstado" > verbaEstado</Label>

<Input id="verbaEstado" {...register("verbaEstado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="verbaCidade" > verbaCidade</Label>

<Input id="verbaCidade" {...register("verbaCidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCentroResultado" > idCentroResultado</Label>

<Input id="idCentroResultado" type = "number" step = "any" {...register("idCentroResultado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="secundaria" > secundaria * </Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="secundaria"
checked = { watch("secundaria") }
onCheckedChange = {(checked) => setValue("secundaria", !!checked)}
              />
  < label htmlFor = "secundaria" className = "text-sm font-normal" >
    { watch("secundaria") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="geradoPeloProcesso" > geradoPeloProcesso</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="geradoPeloProcesso"
checked = { watch("geradoPeloProcesso") }
onCheckedChange = {(checked) => setValue("geradoPeloProcesso", !!checked)}
              />
  < label htmlFor = "geradoPeloProcesso" className = "text-sm font-normal" >
    { watch("geradoPeloProcesso") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="sequenciaHerdeiro" > sequenciaHerdeiro</Label>

<Input id="sequenciaHerdeiro" type = "number" step = "any" {...register("sequenciaHerdeiro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUnidade" > idUnidade</Label>

<Input id="idUnidade" type = "number" step = "any" {...register("idUnidade") } />

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
