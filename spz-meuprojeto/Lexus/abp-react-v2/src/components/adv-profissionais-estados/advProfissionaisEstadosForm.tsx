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
  import { useCreateAdvProfissionaisEstados, useUpdateAdvProfissionaisEstados } from "@/lib/abp/hooks/useAdvProfissionaisEstadoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProfissionalEstado: z.any(),
  
    idProfissional: z.any(),
  
    estado: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProfissionaisEstadosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProfissionaisEstadosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProfissionaisEstadosFormProps) {
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

  const createMutation = useCreateAdvProfissionaisEstados();
const updateMutation = useUpdateAdvProfissionaisEstados();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProfissionaisEstados updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProfissionaisEstados created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprofissionaisestados:", error);
    toast.error(error.message || "Failed to save advprofissionaisestados");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProfissionaisEstados": "Create advProfissionaisEstados" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprofissionaisestados." : "Fill in the details to create a new advprofissionaisestados." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProfissionalEstado" > idProfissionalEstado</Label>

<Input id="idProfissionalEstado" type = "number" step = "any" {...register("idProfissionalEstado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProfissional" > idProfissional * </Label>

<Input id="idProfissional" type = "number" step = "any" {...register("idProfissional") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="estado" > estado</Label>

<Input id="estado" {...register("estado") } />

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
