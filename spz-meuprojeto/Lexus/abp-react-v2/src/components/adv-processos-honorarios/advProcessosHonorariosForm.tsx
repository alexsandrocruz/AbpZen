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
  import { useCreateAdvProcessosHonorarios, useUpdateAdvProcessosHonorarios } from "@/lib/abp/hooks/useAdvProcessosHonorarioses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idHonorario: z.any(),
  
    idProcesso: z.any(),
  
    dataPrevistaClienteReceber: z.any(),
  
    rpv: z.any(),
  
    precatorio: z.any(),
  
    nrParcelasProcesso: z.any(),
  
    valorHonorarios: z.any(),
  
    valorHonorariosTipo: z.any(),
  
    honorariosTextoFicha: z.any(),
  
    valorHonorariosDestaque: z.any(),
  
    valorHonorariosDestaqueTipo: z.any(),
  
    dataPrevisaoHonorariosDestaque: z.any(),
  
    imposto: z.any(),
  
    complementoPositivo: z.any(),
  
    sucumbencia: z.any(),
  
    saldoDevedor: z.any(),
  
    valorDeferido: z.any(),
  
    dataLiberacaoValorDeferido: z.any(),
  
    idConta: z.any(),
  
    dataPrevisaoRepasseCliente: z.any(),
  
    idContaPagar: z.any(),
  
    formaRecebimento: z.any(),
  
    nrParcelasSomenteSucumbencia: z.any(),
  
    sucumbenciaAdd: z.any(),
  
    sucumbenciaAddData: z.any(),
  
    sucumbenciaAddIdBanco: z.any(),
  
    boleto: z.any(),
  
    emitir: z.any(),
  
    emitido: z.any(),
  
    nfComComplementoPositivo: z.any(),
  
    bancarioCpf: z.any(),
  
    herdeirosTipoValor: z.any(),
  
    bancarioPerc: z.any(),
  
    tarifa: z.any(),
  
    tarifaParcelas: z.any(),
  
    bancarioFavorecido: z.any(),
  
    bancarioBancoId: z.any(),
  
    bancarioTipoConta: z.any(),
  
    bancarioAgencia: z.any(),
  
    bancarioConta: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    incluidoPor: z.any(),
  
    tsAlteracao: z.any(),
  
    alteradoPor: z.any(),
  
    valorHonorariosDestaqueSomente: z.any(),
  
    valorHonorariosDestaqueTipoSomente: z.any(),
  
    dataPrevisaoHonorariosDestaqueSomente: z.any(),
  
    idBancoDestaqueSomente: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProcessosHonorariosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProcessosHonorariosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProcessosHonorariosFormProps) {
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

  const createMutation = useCreateAdvProcessosHonorarios();
const updateMutation = useUpdateAdvProcessosHonorarios();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProcessosHonorarios updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProcessosHonorarios created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprocessoshonorarios:", error);
    toast.error(error.message || "Failed to save advprocessoshonorarios");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProcessosHonorarios": "Create advProcessosHonorarios" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprocessoshonorarios." : "Fill in the details to create a new advprocessoshonorarios." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idHonorario" > idHonorario</Label>

<Input id="idHonorario" type = "number" step = "any" {...register("idHonorario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso * </Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevistaClienteReceber" > dataPrevistaClienteReceber</Label>

<Input id="dataPrevistaClienteReceber" {...register("dataPrevistaClienteReceber") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="rpv" > rpv</Label>

<Input id="rpv" {...register("rpv") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="precatorio" > precatorio</Label>

<Input id="precatorio" {...register("precatorio") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nrParcelasProcesso" > nrParcelasProcesso</Label>

<Input id="nrParcelasProcesso" type = "number" step = "any" {...register("nrParcelasProcesso") } />

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
  <Label htmlFor="honorariosTextoFicha" > honorariosTextoFicha</Label>

<Input id="honorariosTextoFicha" {...register("honorariosTextoFicha") } />

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
  <Label htmlFor="imposto" > imposto</Label>

<Input id="imposto" type = "number" step = "any" {...register("imposto") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="complementoPositivo" > complementoPositivo</Label>

<Input id="complementoPositivo" type = "number" step = "any" {...register("complementoPositivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sucumbencia" > sucumbencia</Label>

<Input id="sucumbencia" type = "number" step = "any" {...register("sucumbencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="saldoDevedor" > saldoDevedor</Label>

<Input id="saldoDevedor" type = "number" step = "any" {...register("saldoDevedor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorDeferido" > valorDeferido</Label>

<Input id="valorDeferido" type = "number" step = "any" {...register("valorDeferido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataLiberacaoValorDeferido" > dataLiberacaoValorDeferido</Label>

<Input id="dataLiberacaoValorDeferido" {...register("dataLiberacaoValorDeferido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idConta" > idConta</Label>

<Input id="idConta" type = "number" step = "any" {...register("idConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevisaoRepasseCliente" > dataPrevisaoRepasseCliente</Label>

<Input id="dataPrevisaoRepasseCliente" {...register("dataPrevisaoRepasseCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idContaPagar" > idContaPagar</Label>

<Input id="idContaPagar" type = "number" step = "any" {...register("idContaPagar") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="formaRecebimento" > formaRecebimento</Label>

<Input id="formaRecebimento" {...register("formaRecebimento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="nrParcelasSomenteSucumbencia" > nrParcelasSomenteSucumbencia</Label>

<Input id="nrParcelasSomenteSucumbencia" type = "number" step = "any" {...register("nrParcelasSomenteSucumbencia") } />

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
  <Label htmlFor="bancarioCpf" > bancarioCpf</Label>

<Input id="bancarioCpf" {...register("bancarioCpf") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="herdeirosTipoValor" > herdeirosTipoValor</Label>

<Input id="herdeirosTipoValor" {...register("herdeirosTipoValor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioPerc" > bancarioPerc</Label>

<Input id="bancarioPerc" type = "number" step = "any" {...register("bancarioPerc") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tarifa" > tarifa</Label>

<Input id="tarifa" type = "number" step = "any" {...register("tarifa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tarifaParcelas" > tarifaParcelas</Label>

<Input id="tarifaParcelas" {...register("tarifaParcelas") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioFavorecido" > bancarioFavorecido</Label>

<Input id="bancarioFavorecido" {...register("bancarioFavorecido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bancarioBancoId" > bancarioBancoId</Label>

<Input id="bancarioBancoId" type = "number" step = "any" {...register("bancarioBancoId") } />

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
  <Label htmlFor="incluidoPor" > incluidoPor</Label>

<Input id="incluidoPor" {...register("incluidoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="alteradoPor" > alteradoPor</Label>

<Input id="alteradoPor" {...register("alteradoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorHonorariosDestaqueSomente" > valorHonorariosDestaqueSomente</Label>

<Input id="valorHonorariosDestaqueSomente" type = "number" step = "any" {...register("valorHonorariosDestaqueSomente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="valorHonorariosDestaqueTipoSomente" > valorHonorariosDestaqueTipoSomente</Label>

<Input id="valorHonorariosDestaqueTipoSomente" {...register("valorHonorariosDestaqueTipoSomente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataPrevisaoHonorariosDestaqueSomente" > dataPrevisaoHonorariosDestaqueSomente</Label>

<Input id="dataPrevisaoHonorariosDestaqueSomente" {...register("dataPrevisaoHonorariosDestaqueSomente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idBancoDestaqueSomente" > idBancoDestaqueSomente</Label>

<Input id="idBancoDestaqueSomente" type = "number" step = "any" {...register("idBancoDestaqueSomente") } />

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
