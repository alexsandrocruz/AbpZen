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
  import { useCreateAdvClientesConvertidos, useUpdateAdvClientesConvertidos } from "@/lib/abp/hooks/useAdvClientesConvertidoses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idRegistro: z.any(),
  
    idCliente: z.any(),
  
    data: z.any(),
  
    tsInclusao: z.any(),
  
    convertidoPor: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvClientesConvertidosFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvClientesConvertidosForm({
  isOpen,
  onClose,
  initialValues,
}: AdvClientesConvertidosFormProps) {
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

  const createMutation = useCreateAdvClientesConvertidos();
const updateMutation = useUpdateAdvClientesConvertidos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advClientesConvertidos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advClientesConvertidos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advclientesconvertidos:", error);
    toast.error(error.message || "Failed to save advclientesconvertidos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advClientesConvertidos": "Create advClientesConvertidos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advclientesconvertidos." : "Fill in the details to create a new advclientesconvertidos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idRegistro" > idRegistro</Label>

<Input id="idRegistro" type = "number" step = "any" {...register("idRegistro") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="data" > data * </Label>

<Input id="data" {...register("data") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="convertidoPor" > convertidoPor</Label>

<Input id="convertidoPor" {...register("convertidoPor") } />

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
