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
  import { useCreateFabEstados, useUpdateFabEstados } from "@/lib/abp/hooks/useFabEstadoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idEstado: z.any(),
  
    sigla: z.any(),
  
    descricao: z.any(),
  
    idPais: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabEstadosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabEstadosForm({
  isOpen,
  onClose,
  initialValues,
}: FabEstadosFormProps) {
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

  const createMutation = useCreateFabEstados();
const updateMutation = useUpdateFabEstados();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabEstados updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabEstados created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabestados:", error);
    toast.error(error.message || "Failed to save fabestados");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabEstados": "Create fabEstados" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabestados." : "Fill in the details to create a new fabestados." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idEstado" > idEstado</Label>

<Input id="idEstado" type = "number" step = "any" {...register("idEstado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sigla" > sigla</Label>

<Input id="sigla" {...register("sigla") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPais" > idPais</Label>

<Input id="idPais" type = "number" step = "any" {...register("idPais") } />

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
