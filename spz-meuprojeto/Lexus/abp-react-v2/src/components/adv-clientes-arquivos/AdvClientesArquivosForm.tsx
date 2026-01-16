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
  import { useCreateAdvClientesArquivos, useUpdateAdvClientesArquivos } from "@/lib/abp/hooks/useAdvClientesArquivoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idArquivo: z.any(),
  
    idCliente: z.any(),
  
    idTipoArquivo: z.any(),
  
    descricao: z.any(),
  
    arquivo: z.any(),
  
    incluidoPor: z.any(),
  
    alteradoPor: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    idProcesso: z.any(),
  
    precisaRevisao: z.any(),
  
    idSolicitante: z.any(),
  
    solicitanteComentario: z.any(),
  
    idRevisor: z.any(),
  
    revisorComentario: z.any(),
  
    reprovado: z.any(),
  
    pendenteVisualizacaoAprovacao: z.any(),
  
    status: z.any(),
  
    autoFTP: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientesArquivosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientesArquivosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientesArquivosFormProps) {
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

  const createMutation = useCreateAdvClientesArquivos();
const updateMutation = useUpdateAdvClientesArquivos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientesArquivos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientesArquivos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclientesarquivos:", error);
    toast.error(error.message || "Failed to save advclientesarquivos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientesArquivos": "Create advClientesArquivos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclientesarquivos." : "Fill in the details to create a new advclientesarquivos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idArquivo" > idArquivo</Label>

<Input id="idArquivo" type = "number" step = "any" {...register("idArquivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipoArquivo" > idTipoArquivo * </Label>

<Input id="idTipoArquivo" type = "number" step = "any" {...register("idTipoArquivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="arquivo" > arquivo</Label>

<Input id="arquivo" {...register("arquivo") } />

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
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="precisaRevisao" > precisaRevisao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="precisaRevisao"
checked = { watch("precisaRevisao") }
onCheckedChange = {(checked) => setValue("precisaRevisao", !!checked)}
              />
  < label htmlFor = "precisaRevisao" className = "text-sm font-normal" >
    { watch("precisaRevisao") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="idSolicitante" > idSolicitante</Label>

<Input id="idSolicitante" type = "number" step = "any" {...register("idSolicitante") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="solicitanteComentario" > solicitanteComentario</Label>

<Input id="solicitanteComentario" {...register("solicitanteComentario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idRevisor" > idRevisor</Label>

<Input id="idRevisor" type = "number" step = "any" {...register("idRevisor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="revisorComentario" > revisorComentario</Label>

<Input id="revisorComentario" {...register("revisorComentario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="reprovado" > reprovado</Label>

<Input id="reprovado" type = "number" step = "any" {...register("reprovado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pendenteVisualizacaoAprovacao" > pendenteVisualizacaoAprovacao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pendenteVisualizacaoAprovacao"
checked = { watch("pendenteVisualizacaoAprovacao") }
onCheckedChange = {(checked) => setValue("pendenteVisualizacaoAprovacao", !!checked)}
              />
  < label htmlFor = "pendenteVisualizacaoAprovacao" className = "text-sm font-normal" >
    { watch("pendenteVisualizacaoAprovacao") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="status" > status</Label>

<Input id="status" {...register("status") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="autoFTP" > autoFTP</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="autoFTP"
checked = { watch("autoFTP") }
onCheckedChange = {(checked) => setValue("autoFTP", !!checked)}
              />
  < label htmlFor = "autoFTP" className = "text-sm font-normal" >
    { watch("autoFTP") ?"Enabled": "Disabled" }
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
