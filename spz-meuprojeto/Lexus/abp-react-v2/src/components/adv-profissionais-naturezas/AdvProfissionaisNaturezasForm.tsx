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
  import { useCreateAdvProfissionaisNaturezas, useUpdateAdvProfissionaisNaturezas } from "@/lib/abp/hooks/useAdvProfissionaisNaturezases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProfissionalNatureza: z.any(),
  
    idProfissional: z.any(),
  
    idNatureza: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProfissionaisNaturezasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProfissionaisNaturezasForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProfissionaisNaturezasFormProps) {
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

  const createMutation = useCreateAdvProfissionaisNaturezas();
const updateMutation = useUpdateAdvProfissionaisNaturezas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProfissionaisNaturezas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProfissionaisNaturezas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprofissionaisnaturezas:", error);
    toast.error(error.message || "Failed to save advprofissionaisnaturezas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProfissionaisNaturezas": "Create advProfissionaisNaturezas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprofissionaisnaturezas." : "Fill in the details to create a new advprofissionaisnaturezas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProfissionalNatureza" > idProfissionalNatureza</Label>

<Input id="idProfissionalNatureza" type = "number" step = "any" {...register("idProfissionalNatureza") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProfissional" > idProfissional * </Label>

<Input id="idProfissional" type = "number" step = "any" {...register("idProfissional") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idNatureza" > idNatureza * </Label>

<Input id="idNatureza" type = "number" step = "any" {...register("idNatureza") } />

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
