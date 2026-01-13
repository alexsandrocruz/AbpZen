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
  import { useCreateAdvTarefas, useUpdateAdvTarefas } from "@/lib/abp/hooks/useAdvTarefases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idTarefa: z.any(),
  
    idTipoTarefa: z.any(),
  
    idCompromisso: z.any(),
  
    idProcesso: z.any(),
  
    dataCadastro: z.any(),
  
    dataParaFinalizacao: z.any(),
  
    descricao: z.any(),
  
    idResponsavel: z.any(),
  
    idExecutor: z.any(),
  
    finalizado: z.any(),
  
    tsFinalizacao: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    incluidoPor: z.any(),
  
    alteradoPor: z.any(),
  
    agendada: z.any(),
  
    horarioInicial: z.any(),
  
    horarioFinal: z.any(),
  
    onde: z.any(),
  
    idCliente: z.any(),
  
    idUsuarioFinalizou: z.any(),
  
    lembreteQuandoFinalizarPara: z.any(),
  
    tecnica: z.any(),
  
    coletivoOriginal: z.any(),
  
    coletivoIdOriginal: z.any(),
  
    coletivoIdCliente: z.any(),
  
    pauta: z.any(),
  
    pautaIdUsuarioResp: z.any(),
  
    pautaRespAceite: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvTarefasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvTarefasForm({
  isOpen,
  onClose,
  initialValues,
}: AdvTarefasFormProps) {
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

  const createMutation = useCreateAdvTarefas();
const updateMutation = useUpdateAdvTarefas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advTarefas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advTarefas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advtarefas:", error);
    toast.error(error.message || "Failed to save advtarefas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advTarefas": "Create advTarefas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advtarefas." : "Fill in the details to create a new advtarefas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idTarefa" > idTarefa</Label>

<Input id="idTarefa" type = "number" step = "any" {...register("idTarefa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipoTarefa" > idTipoTarefa * </Label>

<Input id="idTipoTarefa" type = "number" step = "any" {...register("idTipoTarefa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCompromisso" > idCompromisso</Label>

<Input id="idCompromisso" type = "number" step = "any" {...register("idCompromisso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataCadastro" > dataCadastro</Label>

<Input id="dataCadastro" {...register("dataCadastro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dataParaFinalizacao" > dataParaFinalizacao</Label>

<Input id="dataParaFinalizacao" {...register("dataParaFinalizacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idResponsavel" > idResponsavel</Label>

<Input id="idResponsavel" type = "number" step = "any" {...register("idResponsavel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idExecutor" > idExecutor</Label>

<Input id="idExecutor" type = "number" step = "any" {...register("idExecutor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="finalizado" > finalizado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="finalizado"
checked = { watch("finalizado") }
onCheckedChange = {(checked) => setValue("finalizado", !!checked)}
              />
  < label htmlFor = "finalizado" className = "text-sm font-normal" >
    { watch("finalizado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsFinalizacao" > tsFinalizacao</Label>

<Input id="tsFinalizacao" type = "date" {...register("tsFinalizacao") } />

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
  <Label htmlFor="incluidoPor" > incluidoPor</Label>

<Input id="incluidoPor" {...register("incluidoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="alteradoPor" > alteradoPor</Label>

<Input id="alteradoPor" {...register("alteradoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="agendada" > agendada</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="agendada"
checked = { watch("agendada") }
onCheckedChange = {(checked) => setValue("agendada", !!checked)}
              />
  < label htmlFor = "agendada" className = "text-sm font-normal" >
    { watch("agendada") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="horarioInicial" > horarioInicial</Label>

<Input id="horarioInicial" type = "number" step = "any" {...register("horarioInicial") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="horarioFinal" > horarioFinal</Label>

<Input id="horarioFinal" type = "number" step = "any" {...register("horarioFinal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="onde" > onde</Label>

<Input id="onde" {...register("onde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente</Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioFinalizou" > idUsuarioFinalizou</Label>

<Input id="idUsuarioFinalizou" type = "number" step = "any" {...register("idUsuarioFinalizou") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="lembreteQuandoFinalizarPara" > lembreteQuandoFinalizarPara</Label>

<Input id="lembreteQuandoFinalizarPara" type = "number" step = "any" {...register("lembreteQuandoFinalizarPara") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tecnica" > tecnica</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="tecnica"
checked = { watch("tecnica") }
onCheckedChange = {(checked) => setValue("tecnica", !!checked)}
              />
  < label htmlFor = "tecnica" className = "text-sm font-normal" >
    { watch("tecnica") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="coletivoOriginal" > coletivoOriginal</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="coletivoOriginal"
checked = { watch("coletivoOriginal") }
onCheckedChange = {(checked) => setValue("coletivoOriginal", !!checked)}
              />
  < label htmlFor = "coletivoOriginal" className = "text-sm font-normal" >
    { watch("coletivoOriginal") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="coletivoIdOriginal" > coletivoIdOriginal</Label>

<Input id="coletivoIdOriginal" type = "number" step = "any" {...register("coletivoIdOriginal") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="coletivoIdCliente" > coletivoIdCliente</Label>

<Input id="coletivoIdCliente" type = "number" step = "any" {...register("coletivoIdCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pauta" > pauta</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pauta"
checked = { watch("pauta") }
onCheckedChange = {(checked) => setValue("pauta", !!checked)}
              />
  < label htmlFor = "pauta" className = "text-sm font-normal" >
    { watch("pauta") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="pautaIdUsuarioResp" > pautaIdUsuarioResp</Label>

<Input id="pautaIdUsuarioResp" type = "number" step = "any" {...register("pautaIdUsuarioResp") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pautaRespAceite" > pautaRespAceite</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pautaRespAceite"
checked = { watch("pautaRespAceite") }
onCheckedChange = {(checked) => setValue("pautaRespAceite", !!checked)}
              />
  < label htmlFor = "pautaRespAceite" className = "text-sm font-normal" >
    { watch("pautaRespAceite") ?"Enabled": "Disabled" }
    </label>
    </div>

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
