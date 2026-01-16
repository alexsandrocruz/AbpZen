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
  import { useCreateAdvRevisaoDocumentos, useUpdateAdvRevisaoDocumentos } from "@/lib/abp/hooks/useAdvRevisaoDocumentoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idRevisao: z.any(),
  
    idUsuarioSolicitante: z.any(),
  
    idUsuarioRevisor: z.any(),
  
    localRede: z.any(),
  
    pendente: z.any(),
  
    aprovado: z.any(),
  
    reprovado: z.any(),
  
    finalizado: z.any(),
  
    comentariosSolicitante: z.any(),
  
    comentariosRevisor: z.any(),
  
    tsInclusao: z.any(),
  
    incluidoPor: z.any(),
  
    tsAlteracao: z.any(),
  
    alteradoPor: z.any(),
  
    ativo: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvRevisaoDocumentosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvRevisaoDocumentosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvRevisaoDocumentosFormProps) {
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

  const createMutation = useCreateAdvRevisaoDocumentos();
const updateMutation = useUpdateAdvRevisaoDocumentos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advRevisaoDocumentos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advRevisaoDocumentos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advrevisaodocumentos:", error);
    toast.error(error.message || "Failed to save advrevisaodocumentos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advRevisaoDocumentos": "Create advRevisaoDocumentos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advrevisaodocumentos." : "Fill in the details to create a new advrevisaodocumentos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idRevisao" > idRevisao</Label>

<Input id="idRevisao" type = "number" step = "any" {...register("idRevisao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioSolicitante" > idUsuarioSolicitante * </Label>

<Input id="idUsuarioSolicitante" type = "number" step = "any" {...register("idUsuarioSolicitante") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuarioRevisor" > idUsuarioRevisor * </Label>

<Input id="idUsuarioRevisor" type = "number" step = "any" {...register("idUsuarioRevisor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="localRede" > localRede</Label>

<Input id="localRede" {...register("localRede") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pendente" > pendente</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pendente"
checked = { watch("pendente") }
onCheckedChange = {(checked) => setValue("pendente", !!checked)}
              />
  < label htmlFor = "pendente" className = "text-sm font-normal" >
    { watch("pendente") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="aprovado" > aprovado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="aprovado"
checked = { watch("aprovado") }
onCheckedChange = {(checked) => setValue("aprovado", !!checked)}
              />
  < label htmlFor = "aprovado" className = "text-sm font-normal" >
    { watch("aprovado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="reprovado" > reprovado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="reprovado"
checked = { watch("reprovado") }
onCheckedChange = {(checked) => setValue("reprovado", !!checked)}
              />
  < label htmlFor = "reprovado" className = "text-sm font-normal" >
    { watch("reprovado") ?"Enabled": "Disabled" }
    </label>
    </div>

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
  <Label htmlFor="comentariosSolicitante" > comentariosSolicitante</Label>

<Input id="comentariosSolicitante" {...register("comentariosSolicitante") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="comentariosRevisor" > comentariosRevisor</Label>

<Input id="comentariosRevisor" {...register("comentariosRevisor") } />

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
