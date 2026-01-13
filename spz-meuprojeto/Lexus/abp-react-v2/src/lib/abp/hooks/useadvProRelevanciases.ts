import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProRelevanciasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProRelevanciases(input: GetAdvProRelevanciasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProRelevanciases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-relevancias", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvProRelevanciases() {
  return useQuery({
    queryKey: ["advProRelevanciases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-relevancias", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProRelevancias(id: string) {
  return useQuery({
    queryKey: ["advProRelevancias", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-relevancias/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProRelevancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-relevancias", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProRelevanciases"] });
    },
  });
}

export function useUpdateAdvProRelevancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-relevancias/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProRelevanciases"] });
      queryClient.invalidateQueries({ queryKey: ["advProRelevancias", data.id] });
    },
  });
}

export function useDeleteAdvProRelevancias() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-relevancias/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProRelevanciases"] });
    },
  });
}


