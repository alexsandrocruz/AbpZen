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
  import { useCreateLogAcoes, useUpdateLogAcoes } from "@/lib/abp/hooks/useLogAcoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idLog: z.any(),
  
    area: z.any(),
  
    acao: z.any(),
  
    usuario: z.any(),
  
    motivo: z.any(),
  
    tsInclusao: z.any(),
  
    idCliente: z.any(),
  
    idProcesso: z.any(),
  
    idCompromisso: z.any(),
  
    idTarefa: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LogAcoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LogAcoesForm({
  isOpen,
  onClose,
  initialValues,
}: LogAcoesFormProps) {
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

  const createMutation = useCreateLogAcoes();
const updateMutation = useUpdateLogAcoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("logAcoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("logAcoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save logacoes:", error);
    toast.error(error.message || "Failed to save logacoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit logAcoes": "Create logAcoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the logacoes." : "Fill in the details to create a new logacoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idLog" > idLog</Label>

<Input id="idLog" type = "number" step = "any" {...register("idLog") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="area" > area</Label>

<Input id="area" {...register("area") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="acao" > acao</Label>

<Input id="acao" {...register("acao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="usuario" > usuario</Label>

<Input id="usuario" {...register("usuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="motivo" > motivo</Label>

<Input id="motivo" {...register("motivo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente</Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso</Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCompromisso" > idCompromisso</Label>

<Input id="idCompromisso" type = "number" step = "any" {...register("idCompromisso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTarefa" > idTarefa</Label>

<Input id="idTarefa" type = "number" step = "any" {...register("idTarefa") } />

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
