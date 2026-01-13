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
  import { useCreateUsuDistancias, useUpdateUsuDistancias } from "@/lib/abp/hooks/useUsuDistanciases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idDistancia: z.any(),
  
    idUsuario: z.any(),
  
    estado: z.any(),
  
    cidade: z.any(),
  
    km: z.any(),
  
    tsInclusao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface UsuDistanciasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function UsuDistanciasForm({
  isOpen,
  onClose,
  initialValues,
}: UsuDistanciasFormProps) {
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

  const createMutation = useCreateUsuDistancias();
const updateMutation = useUpdateUsuDistancias();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("usuDistancias updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("usuDistancias created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save usudistancias:", error);
    toast.error(error.message || "Failed to save usudistancias");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit usuDistancias": "Create usuDistancias" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the usudistancias." : "Fill in the details to create a new usudistancias." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idDistancia" > idDistancia</Label>

<Input id="idDistancia" type = "number" step = "any" {...register("idDistancia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

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
  <Label htmlFor="km" > km</Label>

<Input id="km" type = "number" step = "any" {...register("km") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

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
