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
  import { useCreateAdvProcessosClientes, useUpdateAdvProcessosClientes } from "@/lib/abp/hooks/useAdvProcessosClienteses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProcessoCliente: z.any(),
  
    idProcesso: z.any(),
  
    idCliente: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProcessosClientesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProcessosClientesForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProcessosClientesFormProps) {
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

  const createMutation = useCreateAdvProcessosClientes();
const updateMutation = useUpdateAdvProcessosClientes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProcessosClientes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProcessosClientes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprocessosclientes:", error);
    toast.error(error.message || "Failed to save advprocessosclientes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProcessosClientes": "Create advProcessosClientes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprocessosclientes." : "Fill in the details to create a new advprocessosclientes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProcessoCliente" > idProcessoCliente</Label>

<Input id="idProcessoCliente" type = "number" step = "any" {...register("idProcessoCliente") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso * </Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idCliente" > idCliente * </Label>

<Input id="idCliente" type = "number" step = "any" {...register("idCliente") } />

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
