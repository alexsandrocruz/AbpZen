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
  import { useCreateAdvClientesAtualizacoes, useUpdateAdvClientesAtualizacoes } from "@/lib/abp/hooks/useAdvClientesAtualizacoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idAtualizacao: z.any(),
  
    idCliente: z.any(),
  
    campo: z.any(),
  
    tsAlteracao: z.any(),
  
    dadoAnterior: z.any(),
  
    idUsuario: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientesAtualizacoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientesAtualizacoesForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientesAtualizacoesFormProps) {
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

  const createMutation = useCreateAdvClientesAtualizacoes();
const updateMutation = useUpdateAdvClientesAtualizacoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientesAtualizacoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientesAtualizacoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclientesatualizacoes:", error);
    toast.error(error.message || "Failed to save advclientesatualizacoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientesAtualizacoes": "Create advClientesAtualizacoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclientesatualizacoes." : "Fill in the details to create a new advclientesatualizacoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idAtualizacao" > idAtualizacao</Label>

<Input id="idAtualizacao" type = "number" step = "any" {...register("idAtualizacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="campo" > campo</Label>

<Input id="campo" {...register("campo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="dadoAnterior" > dadoAnterior</Label>

<Input id="dadoAnterior" {...register("dadoAnterior") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario</Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

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
